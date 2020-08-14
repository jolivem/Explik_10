<?php

error_reporting(E_ALL ^ E_NOTICE);

$servername = "127.0.0.1";
$username = "livef_explik";
$password = "ExMj&218133";
$dbname = "livefre148340fr34204_explik";
error_log("coucou", 0);

// create the file
$myfile = fopen("F:\\Inetpub\\vhosts\\explik.fr\\httpdocs\\sitemap.xml", "w");
if( $myfile == false )
{
   error_log ( "Error in opening new file" , 0 );
   die("Error in opening new file: ");
}
// on se connecte à MySQL
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    error_log("mysqlin failed: " . $conn->connect_error, 0);
	die("Connection failed: " . $conn->connect_error);
}
error_log("After open connection", 0);

// Add header and main page
fwrite($myfile, "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
fwrite($myfile, "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
fwrite($myfile, "  <url>\n");
fwrite($myfile, "    <loc>https://www.explik.fr/</loc>\n");
fwrite($myfile, "  </url>\n");

// on crée la requête SQL
$sql = 'SELECT * FROM explik_pages WHERE IsLocked="0" AND IsControlled="1" AND IsRejected="0"';
//error_log("bloub", 0);
$result = $conn->query($sql);
//error_log( "cuiui", 0);
if ($result->num_rows > 0) {
	error_log( "num_rows=" . $result->num_rows, 0);
	
    // output data of each row
    while($row = $result->fetch_assoc()) {
		fwrite($myfile, "  <url>\n");
		$loc = "    <loc>https://www.explik.fr/wiki/" . $row["Id"] . "/" . rawurlencode($row["Title"]) . "</loc>\n";
		fwrite($myfile, $loc);
		$datetime = explode(" ", $row["PublishedOn"]);
		$mod = "    <lastmod>" . $datetime[0] . "</lastmod>\n";
		fwrite($myfile, $mod);
		
		fwrite($myfile, "  </url>\n");
    }
} else {
    error_log( "0 results", 0);
}
fwrite($myfile, "</urlset>");
fclose($myfile);
$conn->close();
error_log("After close connection", 0);
?>