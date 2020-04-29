Imports System.Management
Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class Server
	Dim server_id As Integer
	Dim host_ip As String
	Dim username As String
	Dim password As String

	Dim main_drive_usage As Double = 0
	Dim second_drive_usage As Double = 0
	Dim cpu_usage As Double = 0
	Dim ram_usage As Double = 0
	Dim network_usage As Double = 0

	Dim myManagementScope As ManagementScope

	Public Sub New(ByVal id As Integer, ByVal ip As String, ByVal user As String, ByVal pwd As String)
		server_id = id
		host_ip = ip
		username = user
		password = pwd
	End Sub

	Public Function ConnectToServer() As Boolean
		' Konfiguration der WMI-Connection
		Dim connectOptions As New System.Management.ConnectionOptions
		With connectOptions
			.Impersonation = ImpersonationLevel.Impersonate
			.Authentication = AuthenticationLevel.Packet
			.Username = username
			.Password = password
		End With

		Try
			myManagementScope = New ManagementScope("\\" & host_ip & "\root\cimv2", connectOptions)
			myManagementScope.Connect()
		Catch ex As Exception
			Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "The connection to the server failed: " & ex.Message)
			Return False
		End Try

		Return True
	End Function

	Public Sub CloseConnection()
		myManagementScope = Nothing
	End Sub

	Public Function FetchServerUsage() As Boolean
		Dim isConnected = False

		If myManagementScope Is Nothing Then
			isConnected = ConnectToServer()
		ElseIf myManagementScope.IsConnected = False Then
			isConnected = ConnectToServer()
		Else
			isConnected = myManagementScope.IsConnected
		End If

		If isConnected = False Then
			Return False
		End If

		Try
			'Drive-Capacity
			Dim query_drive As New Management.ObjectQuery("SELECT * FROM Win32_Volume")
			Dim searcher_drive As New Management.ManagementObjectSearcher(myManagementScope, query_drive)
			For Each queryObj_drive As Management.ManagementObject In searcher_drive.Get()
				If queryObj_drive("Caption") = "C:\" And Not queryObj_drive("Capacity") = 0 Then
					main_drive_usage = (queryObj_drive("FreeSpace") / queryObj_drive("Capacity")) * 100
				ElseIf Not queryObj_drive("Capacity") = 0 Then
					second_drive_usage = (queryObj_drive("FreeSpace") / queryObj_drive("Capacity")) * 100
				End If
			Next

			'CPU-Usage
			Dim query_cpu As New Management.ObjectQuery("SELECT * FROM Win32_PerfFormattedData_Counters_ProcessorInformation where Name = '_Total'")
			Dim searcher_cpu As New Management.ManagementObjectSearcher(myManagementScope, query_cpu)
			For Each queryObj_cpu As Management.ManagementObject In searcher_cpu.Get()
				cpu_usage = queryObj_cpu("PercentProcessorTime")
			Next

			'RAM-Current-Usage
			Dim ram_current_available As Double
			Dim query_ram As New Management.ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Memory")
			Dim searcher_ram As New Management.ManagementObjectSearcher(myManagementScope, query_ram)
			For Each queryObj_ram As Management.ManagementObject In searcher_ram.Get()
				ram_current_available = queryObj_ram("AvailableMBytes")
			Next

			'RAM-Total
			Dim query_tram As New Management.ObjectQuery("SELECT * FROM Win32_ComputerSystem")
			Dim searcher_tram As New Management.ManagementObjectSearcher(myManagementScope, query_tram)
			For Each queryObj_tram As Management.ManagementObject In searcher_tram.Get()
				ram_usage = ((queryObj_tram("TotalPhysicalMemory") / 1024 / 1024) - ram_current_available) / (queryObj_tram("TotalPhysicalMemory") / 1024 / 1024) * 100
			Next

			'Get Network-Usage
			Dim query_networkusage As New Management.ObjectQuery("Select * from Win32_PerfFormattedData_Tcpip_NetworkInterface")
			Dim searcher_networkusage As New Management.ManagementObjectSearcher(myManagementScope, query_networkusage)
			For Each queryObj_networkusage As Management.ManagementObject In searcher_networkusage.Get()
				If Not queryObj_networkusage("BytesTotalPersec") = 0 Then
					network_usage = queryObj_networkusage("BytesTotalPersec") / (queryObj_networkusage("CurrentBandwidth") / 8) * 100
				End If
			Next
		Catch err As Management.ManagementException
			Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "WMI query failed with the following error: " & err.Message)
			Return False
		Catch unauthorizedErr As System.UnauthorizedAccessException
			Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "Authentication error: " & unauthorizedErr.Message)
			Return False
		Catch ex As Exception
			Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "A general problem occured. Check your and the servers connection to the local network")
			Console.WriteLine(vbTab & vbTab & vbTab & ex.ToString() + vbNewLine)
			Return False
		End Try

		Return True
	End Function

	Public Sub PrintOutCurrentUsage()
		Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "C-Festplatte" + vbTab + main_drive_usage.ToString & "%" & vbNewLine &
						  vbTab & vbTab & vbTab & "D-Festplatte" & vbTab & second_drive_usage.ToString & "%" & vbNewLine &
						  vbTab & vbTab & vbTab & "RAM-Auslastung" & vbTab & ram_usage.ToString & "%" & vbNewLine &
						  vbTab & vbTab & vbTab & "CPU-Auslastung" & vbTab & cpu_usage.ToString & "%" & vbNewLine &
						  vbTab & vbTab & vbTab & "Netzwerk" & vbTab & network_usage.ToString & "%")
	End Sub

	Public Function InsertIntoDB(ByVal mySqlConnection As MySqlConnection) As Boolean
		Try
			Dim myCmd As MySqlCommand = New MySqlCommand
			myCmd.Connection = mySqlConnection

			myCmd.CommandText = "INSERT INTO log (value, server_id, monitoring_type_id) VALUES (" & main_drive_usage.ToString("F04", New CultureInfo("en-us")) & ", " & server_id & ", (SELECT monitoring_type_id FROM monitoring_type WHERE name = 'main_drive_usage'))"
			myCmd.ExecuteNonQuery()

			myCmd.CommandText = "INSERT INTO log (value, server_id, monitoring_type_id) VALUES (" & second_drive_usage.ToString("F04", New CultureInfo("en-us")) & ", " & server_id & ", (SELECT monitoring_type_id FROM monitoring_type WHERE name = 'second_drive_usage'))"
			myCmd.ExecuteNonQuery()

			myCmd.CommandText = "INSERT INTO log (value, server_id, monitoring_type_id) VALUES (" & ram_usage.ToString("F04", New CultureInfo("en-us")) & ", " & server_id & ", (SELECT monitoring_type_id FROM monitoring_type WHERE name = 'ram_usage'))"
			myCmd.ExecuteNonQuery()

			myCmd.CommandText = "INSERT INTO log (value, server_id, monitoring_type_id) VALUES (" & network_usage.ToString("F04", New CultureInfo("en-us")) & ", " & server_id & ", (SELECT monitoring_type_id FROM monitoring_type WHERE name = 'network_usage'))"
			myCmd.ExecuteNonQuery()

			myCmd.CommandText = "INSERT INTO log (value, server_id, monitoring_type_id) VALUES (" & cpu_usage.ToString("F04", New CultureInfo("en-us")) & ", " & server_id & ", (SELECT monitoring_type_id FROM monitoring_type WHERE name = 'cpu_usage'))"
			myCmd.ExecuteNonQuery()
		Catch ex As Exception
			Console.WriteLine(DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss") & vbTab & "A error occured while inserting the current server data into the database." & vbNewLine & vbTab & vbTab & vbTab & "D-Festplatte" & vbTab & ex.Message)
			Return False
		End Try
		Return True
	End Function
End Class
