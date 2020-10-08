window.onload = function () {
    console.log("enter js");
    let elemList = document.getElementsByClassName("add_button");
   /* let elemList = document.getElementsByClassName("product_img");*/
    console.log("enter addToCart.js, elem:" + elemList[1]);
    

    for (let i = 0; i < elemList.length; i++) {
        console.log("enter addToCart.js loop" + elemList[i]);
        elemList[i].addEventListener("click", onAdd);
    }
}

function onAdd(event) {
    console.log("enter onAdd");
    let elem = event.currentTarget;
    let productId = elem.getAttribute("productId");
    sendProctId(productId);
}

function sendProctId(productId) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/Add");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
           

            if (this.status === 200 || this.status === 302) {
                console.log("before parse " + this.responseText);

                let data = JSON.parse(this.responseText);
                console.log("after parse " + data);
                console.log("operation: " + data.status)
                console.log("redirect_url: " + data.url)

                if (data.status === "success") {
                    console.log("Successful stored: " + data.status);
                }
                else if (data.status === "redirect") {
                    window.location = data.url;
                }
            }
        }
    };
    // send productId to Add controller
    xhr.send(JSON.stringify({
        ProductId: productId
    }));
}

