function OrderBtn(btn) {
    var p = +btn.id;
    console.log("clicked")
    order(p);
}

function order(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Orders?check=true&page=" + x,
        data: $("#myOrders").serialize(),
        success: function (data) {
            console.log(data);
            $("#myOrders").html(data);
        }
    });
};