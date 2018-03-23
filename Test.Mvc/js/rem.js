;(function(win){
	var rem,
		tid,
		doc = document,
		docEl = doc.documentElement,
		winWidth = docEl.getBoundingClientRect().width;

	//set rem
	function refreshRem(){
		winWidth = docEl.getBoundingClientRect().width;
		if(winWidth > 640){
			winWidth = 640;
		}
		rem = winWidth / 10;
		docEl.style.fontSize = rem + 'px';
	}

	win.addEventListener('resize', function(){
		clearTimeout(tid);
		tid = setTimeout(refreshRem, 300);
	}, false);

	refreshRem();

	//css active hack for ios
	doc.addEventListener('touchstart', function(){}, false);
})(window);