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
            background-color: #fff;
            color: #808080;
            font-family: 'Nunito', sans-serif;
            font-weight: 200;
            height: 100vh;
            margin: 0;
        }

        .full-height {
            height: 100vh;
        }

        .flex-center {
            align-items: center;
            display: flex;
            justify-content: center;
        }

        .position-ref {
            position: relative;
        }

        .top-right {
            position: absolute;
            right: 10px;
            top: 18px;
        }

        .content {
            text-align: center;
        }

        .title {
            font-size: 84px;
            color: #FB431C;
        }

        .font {
            font-size: 18px;
        }

        .m-b-md {
            margin-bottom: 30px;
        }


    </style>
</head>
<body>

<nav class="navbar navbar-dark bg-dark">
    <a class="navbar-brand">Server-Monitoring</a>
    <a type="button" class="btn btn-secondary" href="{{config('app.url', '')}}/">Zurück</a>
</nav>

<ul class="list-group">
    <li class="list-group-item active">Ausgewählter Server: {{$server->server_name}}</li>
    <li class="list-group-item">IP-Adresse: {{$server->ip_adresse}}</li>
    <li class="list-group-item">Standort: {{$server->location}}</li>
    <li class="list-group-item">Beginn: {{$begin}}</li>
    <li class="list-group-item">Ende: {{$end}}</li>
</ul>
<br>

<ul class="list-group">
    <li class="list-group-item active">Protokolldaten des Servers im ausgewähten Zeitraum:</li>
</ul>
@foreach($logs as $log)
    @foreach($monitoring_types as $monitoring_type)
<ul class="list-group">
    <li class="list-group-item list-group-item-dark">Überwachungsparameter: {{$monitoring_type->name}}</li>
    <li class="list-group-item">momentaner Wert: {{$log->value}}</li>
    <li class="list-group-item">Warnung bei: {{$monitoring_type->warning_value}}</li>
    <li class="list-group-item">Erstellt am: {{$log->created_at}}</li>
    <li class="list-group-item">Server: {{$server->server_name}}</li>
    <li class="list-group-item">Server ID: {{$log->server_id}}</li>
    <br>
</ul>
    @endforeach
@endforeach

</body>
</html>





