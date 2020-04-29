<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Http\Request;

class DataController extends Controller
{

    public function getServer()
    {
        $servers = DB::table('server')->get();
        return view('welcome')->with('servers', $servers);
    }

    public function getServer_ID($id, $begin, $end)
    {
        $server = DB::table('server')->where('server_id', $id)->first();
        $log = DB::table('log')->join('monitoring_type', 'monitoring_type.monitoring_type_id', '=', 'log.monitoring_type_id')->where('server_id', $id)->where('created_at', '>=', $begin)
            ->where('created_at', '<=', $end)->get();
        $monitoring_type = DB::table('monitoring_type')->where('monitoring_type_id', $id)->get();

        return view('server')->with('server', $server)->with('begin', $begin)->with('end', $end)->with('logs', $log)->with('monitoring_types', $monitoring_type);


    }

    public function callServer(Request $request)
    {
        return redirect(url('/server/'.$request->server_id."/".$request->begin."/".$request->end));
    }
}
