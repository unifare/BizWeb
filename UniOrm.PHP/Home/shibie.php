<?php

// place your code here
extract($_POST);
	function suiji()
	{
		$zheng = rand(6,9);
		$xiao = rand(1,9);
		$he = $zheng.".".$xiao;	
		return $he;
	}

	$xindong = suiji();
	$shichang = suiji();
	$hudong = suiji();
	$jiaoyou = suiji();
	$zhuyin = rand(75,96);
	$fuyin1 = rand(75,95);
	$fuyin2 = rand(75,95);
	$fuyin3 = rand(75,95);

	$erweima =  'assets/taobao.png';
	if($gender=="0")
		{
			$assets =  'assets/nan'.rand(1,2).'.png';
		}
	else
		{
			$assets =  'assets/nv'.rand(1,4).'.png';	
		}					
	$assets = imagecreatefrompng($assets);
	$erweima = imagecreatefrompng($erweima);//��ά�� 


	$font ='assets/fzstk.ttf';
	imagefttext($assets, 24, 0, 245, 420, imagecolorallocate($assets,0,0,0), $font, "= ".$name." =");
	imagefttext($assets, 28, 0, 87, 750, imagecolorallocate($assets,0,0,0), $font, $xindong);
	imagefttext($assets, 28, 0, 205, 750, imagecolorallocate($assets,0,0,0), $font, $shichang);
	imagefttext($assets, 28, 0, 325, 750, imagecolorallocate($assets,0,0,0), $font, $hudong);
	imagefttext($assets, 28, 0, 445, 750, imagecolorallocate($assets,0,0,0), $font, $jiaoyou);
	imagefttext($assets, 18, 0, 460, 490, imagecolorallocate($assets,0,0,0), $font, $zhuyin."%");
	imagefttext($assets, 18, 0, 460, 543, imagecolorallocate($assets,0,0,0), $font, $fuyin1."%");
	imagefttext($assets, 18, 0, 460, 595, imagecolorallocate($assets,0,0,0), $font, $fuyin2."%");
	imagefttext($assets, 18, 0, 460, 645, imagecolorallocate($assets,0,0,0), $font, $fuyin3."%");
	$savepath = 'images/'.date('Ym');
	$savename = md5($name.$gender.rand(1,60)).'.jpg';
	$savefile = $savepath .'/'. $savename;
	
	if(!is_dir($savepath)){
		mkdir($savepath,0777,true);
	}


	imagecopymerge($assets, $erweima,400,860, 0, 0, 140, 140, 100);
	imagejpeg($assets, $savefile,85);
	imagedestroy($assets);
	echo $savefile;