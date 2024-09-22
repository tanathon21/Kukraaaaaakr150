<?php
    $user = 'root';
    $pass = '';
    $db = 'testl';

    $db = new mysqli('localhost', $user,$pass,$db) or die ("Unable to connect");
    echo"Great work!!!";
?>