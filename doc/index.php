<?php
// 主域名(也就是当前这个页面) http://www.xxx.com/index.php

// 副域名池 http://www.BBB.com/xxx/yyy/xxx.html
// 副域名池 http://www.CCC.com/xxx/yyy/xxx.html
// 副域名池 http://www.DDD.com/xxx/yyy/xxx.html
//推广链接如下：
//"http://www.xxx.com/index.php?url=http://www.BBB.com/xxx/yyy/xxx.html" ;

$url = $_GET["url"];

//域名池
$urlList =array(0=>"www.BBB.com",1=>"www.CCC.com";2=>"wwww.DDD.com");
 
$arr = range(0,2);

$domain = $urlList[$arr];  //这里可以去调用检测接口，看是否屏蔽，屏蔽了，urlList剔除掉，再随机 ；

$url="http://".$domain."/xxx/yyy/xxx.html";
 
header('Location: $url');

//确保重定向后，后续代码不会被执行 
die;
?>

