<?php

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/','DataController@getServer');
Route::post('/', 'DataController@callServer');

Route::get('/server/{id}/{begin}/{end}', 'DataController@getServer_ID');
//Route::get('/server/{id}/', 'DataController@getServer_ID');

/*Route::post('/', function (Request $request) {
    dd($request);
});*/
