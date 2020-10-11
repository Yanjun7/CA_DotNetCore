window.onload = function ()
{
	let form = document.getElementById("searchform");

	form.onsubmit = function ()
	{
		let searchElem = document.getElementById("search");
		let searchResult = searchElem.value.trim();
		if (searchResult.length !== 0)
		{
			return true;	
		}
		return false;	
	}
}
    
