function livestockBtn(btn) {
    var p = +btn.id;
    console.log("clicked")
    livestockFxn(p);
}

function livestockFxn(x) {
    $.ajax({
        type: "GET",
        url: "/Dashboard/Livestock?check=true&page=" + x,
        data: $("#myLivestock").serialize(),
        success: function (data) {
            console.log(data);
            $("#myLivestock").html(data);
        }
    });
};
