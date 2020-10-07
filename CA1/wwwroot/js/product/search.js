window.onload = function ()
{
	let form = document.getElementById("searchform");
	console.log("Testing: form:" + form);

	form.onsubmit = function ()
	{
		let searchElem = document.getElementById("search");
		console.log("Testing: search:" + searElem);
		
		let searchResult = searchElem.value.trim();
		if (searchResult.length !== 0) {
			
			return true;	
		}

		return false;	
		
	}
}
    
