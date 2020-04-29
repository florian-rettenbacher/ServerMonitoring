<!DOCTYPE html>
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css"
      integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
<script src="https://code.jquery.com/jquery-3.4.1.slim.min.js"
        integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n"
        crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"
        integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo"
        crossorigin="anonymous"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"
        integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6"
        crossorigin="anonymous"></script>

<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Server-Monitoring</title>

    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,600" rel="stylesheet">

    <!-- Styles -->
    <style>
        html, body {

            background-image: url({{ URL::asset('uploads/server.jpeg') }});
            background-size: cover;
            background-position: center;
            background-repeat: repeat;

            color: #808080;
            font-family: 'Nunito', sans-serif;
            font-weight: 200;
            height: 100vh;
            margin: 0;
        }

    </style>
</head>
<nav class="navbar navbar-dark bg-dark">
    <a class="navbar-brand">Server-Monitoring</a>
    <a type="button" class="btn btn-outline-primary" href="{{config('app.url', '')}}/">Zurück</a>
</nav>
<br>

<div class="container">
    <div class="col">
        <ul class="list-group">
            <div class="p-3 mb-2 bg-dark text-white">Allgemeine Serverinformationen:</div>
            <li class="list-group-item">Ausgewählter Server: {{$server->server_name}}</li>
            <li class="list-group-item">IP-Adresse: {{$server->ip_adresse}}</li>
            <li class="list-group-item">Standort: {{$server->location}}</li>
            <li class="list-group-item">Serverdaten von: {{$begin}}</li>
            <li class="list-group-item">Serverdaten bis: {{$end}}</li>
        </ul>
    </div>
</div>

<br>
<div class="container">
    <div class="col">
        <div class="p-3 mb-2 bg-dark text-white">Alle vorhandenen Protokolldaten des Servers im ausgewähten Zeitraum:</div>
            @foreach($logs as $log)
                @foreach($monitoring_types as $monitoring_type)
                <ul class="list-group">
                    <div class="p-3 mb-2 bg-info text-white">Überwachungsparameter: {{$log->name}}</div>
                    <li class="list-group-item">gemessener Wert: {{$log->value}}</li>
                    <li class="list-group-item">Warnung bei: {{$monitoring_type->warning_value}}</li>
                    <li class="list-group-item">Erstellt am: {{$log->created_at}}</li>
                    <li class="list-group-item">Server: {{$server->server_name}}</li>
                    <li class="list-group-item">Server ID: {{$log->server_id}}</li>
                    <br>
                </ul>
            @endforeach
        @endforeach
    </div>
</div>
</body>
</html>





