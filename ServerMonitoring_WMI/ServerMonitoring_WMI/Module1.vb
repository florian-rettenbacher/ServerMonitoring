Imports System.Management
Imports MySql.Data.MySqlClient
Imports System.Globalization

Module Module1
	' @TODO: Exception-Handling, Schöne Log-Ausgabe

	Dim servers As List(Of Server) = New List(Of Server)

	Dim mySqlConnection As New MySqlConnection

	Sub Main()
		If ConnectToDB() Then
			getServers()
		End If

		While True
			updateAllServer()
			Threading.Thread.Sleep(2000)
		End While
	End Sub

	Private Function ConnectToDB() As Boolean
		Try
			Dim csb As MySqlConnectionStringBuilder = New MySqlConnectionStringBuilder With {
				.Server = "localhost",
				.UserID = "root",
				.Database = "server_monitoring",
				.Password = "",
				.PersistSecurityInfo = True
			}
			mySqlConnection.ConnectionString = csb.ConnectionString
			mySqlConnection.Open()
		Catch ex As Exception
			Console.WriteLine("Es konnte keine Verbinung zur Datenbank hergestellt werden.")
			Return False
		End Try
		Return True
	End Function

	Private Sub getServers()
		Dim myCmd As MySqlCommand = New MySqlCommand()
		Dim myReader As MySqlDataReader
		myCmd.Connection = mySqlConnection

		myCmd.CommandText = "SELECT * FROM server"
		myReader = myCmd.ExecuteReader()

		While myReader.Read()
			servers.Add(New Server(myReader.GetInt16("server_id"), myReader.GetString("ip_adresse"), myReader.GetString("username"), myReader.GetString("password")))
		End While

		myReader.Close()
	End Sub

	Private Sub updateAllServer()
		If servers.Count > 0 Then
			For Each server As Server In servers
				server.ConnectToServer()
				server.FetchServerUsage()
				server.CloseConnection()
				server.PrintOutCurrentUsage()
				server.InsertIntoDB(mySqlConnection)
			Next
		End If
	End Sub
End Module