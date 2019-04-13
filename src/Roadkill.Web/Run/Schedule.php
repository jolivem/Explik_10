<?php
$servername = "127.0.0.1";
$username = "livef_explik";
$password = "ExMj&218133";
$dbname = "livefre148340fr34204_explik";
error_log("bobo", 0);
// on se connecte à MySQL
//$db = mysqli_connect('localhost', 'livef_explik', 'ExMj218133');
$conn = new mysqli($servername, $username, $password, $dbname);
error_log("bubu", 0);
// Check connection
if ($conn->connect_error) {
    error_log("mysqlin failed: " . $conn->connect_error, 0);
	die("Connection failed: " . $conn->connect_error);
}
error_log("After open connection", 0);
// on crée la requête SQL
$sql = 'SELECT * FROM explik_pages WHERE IsLocked="0" AND IsControlled="1" AND IsRejected="0"';
//error_log("bloub", 0);
$result = $conn->query($sql);
//error_log( "cuiui", 0);
if ($result->num_rows > 0) {
	error_log( "num_rows=" . $result->num_rows, 0);
	
    // output data of each row
    while($row = $result->fetch_assoc()) {
        //error_log( "nbview: " . $row["Id"] . $row["NbView"], 0);
		$n = rand(1,20) + $row["NbView"];
		//error_log( "nbview: " . $row["NbView"] . "n: " . $n, 0);
		$sql1="UPDATE explik_pages SET NbView='".$n."' WHERE Id='".$row["Id"]."'";
		//error_log( "sql1: " . $sql1, 0);
		$result1=$conn->query($sql1);
    }
} else {
    error_log( "0 results", 0);
}
$conn->close();
error_log("After close connection", 0);
?>