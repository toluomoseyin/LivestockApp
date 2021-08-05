var firebaseConfig = {
    apiKey: "AIzaSyBb_EjcGD9BDyMtnB_IJkQClssUD51n1X0",
    authDomain: "livestockmarket-2496f.firebaseapp.com",
    databaseURL: "https://livestockmarket-2496f-default-rtdb.firebaseio.com",
    projectId: "livestockmarket-2496f",
    storageBucket: "livestockmarket-2496f.appspot.com",
    messagingSenderId: "646621801723",
    appId: "1:646621801723:web:50a219d5b82a297314545e",
    measurementId: "G-CF3XR2G37M"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

//check is the user is logged in
function isLoggedIn() {
    if (currentUser != null && currentUser != undefined && currentUser !== '') {
        return true;
    }
    return false;
}

//Check if the item already exists in the firebase store
function isInFirebaseStore(firebaseStore, ItemId) {
    let exists = false;
    firebaseStore.child(ItemId).once("value", snapshot => {
        if (snapshot.exists()) {
            exists = true;
        }
    })
    return exists;
}

//add item to firebase store if the item does not exist else update only the quantity value
function addItemToFirebaseStore(userId, data) {
    var firebaseStore = firebase.database().ref("cart/" + userId);
    if (isInFirebaseStore(firebaseStore, data.Id)) {
        firebaseStore.child(data.Id).child("quantity").transaction(function (quantity) {
            return quantity = data.quantity
        })
        return;
    }
    firebaseStore.child(data.Id).set(data);
}

//remove item with a specified user id and item id from firebase cart
function removeCartItemFromFirebaseStore(userId, itemId) {
    var firebaseStore = firebase.database().ref("cart/" + userId);
    firebaseStore.child(itemId).remove();
}

function addCartItemToLocalStorage(data) {
    var key = "livestockCart";
    var cartItems = localStorage.getItem(key);
    if (cartItems != null && cartItems != '' && cartItems != undefined) {
        var itemsArray = JSON.parse(cartItems);
        for (let i = 0; i < itemsArray.length; i++) {
            if (itemsArray[i].Id === data.Id) {
                itemsArray[i].quantity = data.quantity;
                var stringItems = JSON.stringify(itemsArray);
                localStorage.setItem(key, stringItems);
                return;
            }
        }
        itemsArray.push(data)
        var stringItems = JSON.stringify(itemsArray);
        localStorage.setItem(key, stringItems);
    } else {
        var items = new Array();
        items.push(data);
        localStorage.setItem(key, JSON.stringify(items));
    }
}

function removeCartItemFromLocalStorage(itemId) {
    var key = "livestockCart";
    var stringItems = localStorage.getItem(key);
    var arrayItems = JSON.parse(stringItems);
    for (let i = 0; i < arrayItems.length; i++) {
        if (arrayItems[i].Id == itemId) {
            arrayItems.splice(i, 1);
            localStorage.setItem(key, JSON.stringify(arrayItems));
            return;
        }
    }
}

function getCartItemsFromFirebase(userId) {
    var items = [];
    var firebaseRef = firebase.database().ref("cart").child(userId);
    firebaseRef.on("value", snapshot => {
        if (snapshot.exists()) {
            var itemsFromFirebase = snapshot.val();
            for (const prop in itemsFromFirebase) {
                items.push(itemsFromFirebase[prop]);
            }
        }
    })
    return items;
}

function getCartItemsFromLocalStorage() {
    var key = "livestockCart";
    let items = [];
    let stringItems = localStorage.getItem(key);
    if (stringItems != null && stringItems != undefined && stringItems != '') {
        items = JSON.parse(stringItems);
    }
    return items;
}

function createCartDropdownItems(cartItems) {
    let html = '';
    for (let i = 0; i < cartItems.length; i++) {
        html += cartDropdownItem(cartItems[i]);
    }
    return html;
}

function moveItemsToFirebaseStore() {
    if (isLoggedIn() && getCartItemsFromLocalStorage().length > 0) {
        let localStorageItems = getCartItemsFromLocalStorage();
        for (let i = 0; i < localStorageItems.length; i++) {
            addItemToFirebaseStore(currentUser, localStorageItems[i]);
        }
        localStorage.removeItem("livestockCart");
    }
}

function updateShoppingCartView(cartItems) {
    var itemDropdown = createCartDropdownItems(cartItems);
    document.querySelector("#shoppingCartContent").innerHTML = itemDropdown;
}

function addToCart(event) {
    event.target.innerHTML = `<i class="fas fa-spinner fa-pulse"></i>`;
    var eventTargetDataset = event.target.dataset;
    var price = eventTargetDataset.price;
    var id = eventTargetDataset.itemId;
    var qty = eventTargetDataset.quantity;
    if (isLoggedIn()) {
        addItemToFirebaseStore(currentUser, { Id: id, price: price, quantity: qty });
    } else {
        addCartItemToLocalStorage({ Id: id, price: price, quantity: qty });
    }
    event.target.innerHTML = `Add to Cart`;
    showAddToCartModal({ Id: id, price: price, quantity: qty });
    updateShoppingCart()
}

function removeFromCart(event) {
    var targetDataset = event.target.dataset;
    if (isLoggedIn()) {
        removeCartItemFromFirebaseStore(currentUser, targetDataset.itemId)
    } else {
        removeCartItemFromLocalStorage(targetDataset.itemId);
    }

    let items = getCartItems();
    updateCartDropdown(items);
}

function updateShoppingCart() {
    moveItemsToFirebaseStore();
    let items = getCartItems();
    if (items == null) {
        items = [];
    }
    updateCartDropdown(items);
}

function getCartItems() {
    let items = [];
    if (isLoggedIn()) {
        items = getCartItemsFromFirebase(currentUser);
    } else {
        items = getCartItemsFromLocalStorage();
    }
    return items;
}

function addItems() {
    updateCartDropdown(items);
}
function updateCartDropdown(items) {
    let html = '';
    let totalAmount = 0;
    let totalItems = items.length;
    for (let i = 0; i < items.length; i++) {
        html += cartDropdownItem(items[i]);
        totalAmount += Number(items[i].price * items[i].quantity);
    }

    if (html == '') {
        html = `
                <div class="row-1 mt-4 ml-6 d-flex justify-content-around">
                <p style="font-size:16px">Your cart is empty</p>
            </div>
                `;
    }
    if (totalAmount === 0) {
        html += `
            <div class="row-2 d-flex justify-content-between">
                <p><strong>Sub-Total</strong></p>
                <h6>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(totalAmount)}</h6>
            </div>
            <div class="row-2 d-flex justify-content-between">
                <p><strong>Total</strong></p>
                <h6>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(totalAmount)}</h6>
            </div>
            <div class="buttons d-flex justify-content-end">
                <button class="btn shift" disabled>View Cart</button>
                <button class="btn" disabled>Checkout</button>
            </div>
            `;
    } else {
        html += `
            <div class="row-2 d-flex justify-content-between">
                <p><strong>Sub-Total</strong></p>
                <h6>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(totalAmount)}</h6>
            </div>
            <div class="row-2 d-flex justify-content-between">
                <p><strong>Total</strong></p>
                <h6>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(totalAmount)}</h6>
            </div>
            <div class="buttons d-flex justify-content-end">
                <button class="btn shift"><a href="/market/shoppingcart" class="text-white">View Cart</a></button>
                <button class="btn"><a href="/market/shippingdetails" class="text-white">Checkout</a></button>
            </div>
            `;
    }

    document.querySelector("#menu-cart").innerHTML = html;
    document.querySelector("#shoppingCart-badge").innerHTML = totalItems;
}
function cartDropdownItem(params) {
    return `
            <div class="row-1 mt-4 ml-6 d-flex justify-content-around">
                <p style="width: 200px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap">ANIMAL ID - ${params.Id} </p>
                <P>${params.quantity}</P>
                <button type="button" data-item-id="${params.Id}" onclick="removeFromCart(event)" class="btn text-white" aria-label="Close">X</button>
            </div>
        `
}

function showAddToCartModal(item) {
    var modalContainer = document.querySelector("#cartModal");
    modalContainer.innerHTML = `<div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                        <button type="button" class="close text-right pr-3" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <div id="" class="px-5 py-3 text-center">
                            <h4 class="site-color"><b>Product Added To Cart</b></h4><br>
                            <div class="border border-grey text-grey px-5 py-3">
                                <i class="fa fa-shopping-bag fa-3x text-green mb-3 text-center justify-content-center overflow-hidden ml-4">
                                    <small id="cart-badge" class="text-white bg-danger rounded-circle">${item.quantity}</small>
                                </i>
                                <h5>The item has been added to your cart</h5>
                                <hr>
                                <h4 class="text-center text-sm-left"><b>Product Details</b></h4>
                                <div class="row">
                                    <div class="col-sm-6 border-md-light-grey-right">
                                        <div class="row">
                                            <div class="col-sm-6 text-sm-left mt-sm-4">
                                                <h6>Quantity: ${item.quantity}</h6><br>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 border-md-light-grey-right m-0 p-0">
                                        <div class="mt-4" style="text-align: center;">
                                            <h6><u>Product Amount</u></h6>
                                            <h6>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(item.price)}</h6>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="mt-4">
                                            <h6><u>Total Amount</u></h6>
                                            <h4 id="total-amount-cart" class="text-green"><b>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(item.price * item.quantity)}</b></h4>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <button class="btn text-white btn-green mt-4" id="ctn"><a href="/market/shippingdetails" class="text-white" style="text-decoration:none;">Proceed To Checkout</a></button>
                        </div>

                </div>
            </div>`;

    $('#cartModal').modal('show');
}

function updateCartTable(container, cartItems) {
    var tableHtml = container.innerHTML;
    var totalPrice = 0;
    for (let i = 0; i < cartItems.length; i++) {
        tableHtml += `
                    <tr>
                        <td>Animal ID - ${cartItems[i].Id}</td>
                        <td>${cartItems[i].quantity}</td>
                        <td>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(cartItems[i].price)}</td>
                        <td>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(cartItems[i].price * cartItems[i].quantity)}</td>
                    </tr>
                    `;
        totalPrice += Number(cartItems[i].price * cartItems[i].quantity);
    }
    tableHtml += `
                    <tr>
                        <td></td>
                        <td></td>
                        <td>Total Amount</td>
                        <td>${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(totalPrice)}</td>
                    </tr>
                    `;
    container.innerHTML = tableHtml;
    document.querySelector("#number-of-items").innerHTML = cartItems.length == 1 ? `(${cartItems.length} Item)` : `(${cartItems.length} Items)`;
    let buttonCont = document.querySelector("#cart-table-container");
    if (cartItems.length > 0) {
        if (buttonCont) {
            buttonCont.innerHTML = `<a href="/market/shippingdetails" class="btn" style="background-color: #06864D; color: white; font-family: inherit">Proceed Checkout</a>`;
        }
    } else {
        if (buttonCont) {
            buttonCont.innerHTML = `<a class="btn" style="background-color: #06864D; color: white; font-family: inherit" disabled>Proceed Checkout</a>`;
        }
    }
}
function fillCartTable() {
    var cartItems = getCartItems();
    if (cartItems == null) {
        cartItems = [];
    }
    var container = document.querySelector("#shopping-cart-table");
    if (container != null) {
        if (isLoggedIn()) {
            let items = [];
            firebase.database().ref("cart/" + currentUser).once('value', (snapshot) => {
                if (snapshot.exists()) {
                    var itemsFromFirebase = snapshot.val();
                    for (const prop in itemsFromFirebase) {
                        items.push(itemsFromFirebase[prop]);
                    }
                }
                updateCartTable(container, items);
            });
        } else {
            updateCartTable(container, cartItems);
        }
    }
}

async function UpdateCheckoutPage() {
    var container = document.querySelector("#cartItemsContainer");
    if (container == null || currentUser == '') return;
    var html = "";
    var price = 0;
    var firebaseRef = await firebase.database().ref("cart").child(currentUser);
    firebaseRef.once("value", snapshot => {
        if (snapshot.exists()) {
            var itemsFromFirebase = snapshot.val();
            let count = 0;
            for (const prop in itemsFromFirebase) {
                price += Number(itemsFromFirebase[prop].price) * Number(itemsFromFirebase[prop].quantity);
                html += `<input type="hidden" name = "orderitems[${count}].livestockid" value = ${itemsFromFirebase[prop].Id} />`;
                html += `<input type="hidden" name = "orderitems[${count}].quantity" value = ${itemsFromFirebase[prop].quantity} />`;
                count++;
            }
            document.querySelector("#itemsPrice").innerHTML = `${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(price)} <input id="hiddenPrice" type="hidden" value = "${price.toFixed(2)}" disabled />`;
            container.innerHTML = html;
        }
    });
}
