<?php
error_reporting(0);
$service_port = 3000;
$address = gethostbyname('127.0.0.1');

$SUR_SPLIT = "{}";
$SPLITTER = $SUR_SPLIT . "1337SPLITTER1337" . $SUR_SPLIT;
$SUB_SPLITTER = $SUR_SPLIT . "1338SPLIT1338" . $SUR_SPLIT;
$STAFF_ID = $SUR_SPLIT . "1339STAFF1339" . $SUR_SPLIT;
        
$socket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP);
if ($socket === false) {
    echo "Server Offline"; exit;
}

$result = socket_connect($socket, $address, $service_port);
if ($result === false) {
    echo "Server Offline"; exit;
}

$dat = "";
while ($out = socket_read($socket, 1024)) {
    $dat = $dat . $out;
}
//echo $dat;
socket_close($socket); 

$splitt = explode($SPLITTER, $dat);
$client_count = $splitt[0];
$players = explode($SUB_SPLITTER, $splitt[1]);
$uptime =$splitt[2];
$most_connected = $splitt[3];

echo '<span style="color:#333; font-size: 20px; font-family: FrancoisOneRegular, OpenSansRegular, Tahoma, sans-serif">';
echo "There are currently $client_count players connected, and the server has been running for $uptime. During this uptime, we have had a peak of $most_connected connected players.<br>";

foreach ($players as $key => $value) {
        if($value != ""){
			$staff = explode($STAFF_ID, $value);
			if(count($staff) > 1){
				echo "{Staff}";
				echo "[" . $staff[1] . "]";//Player is a staff member
			} else {
				echo "[" . $value . "]"; //Player is just a player
			}
        }
}

echo '</span>';
//socket_close($socket);
?>
