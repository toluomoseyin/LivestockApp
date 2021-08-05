$(document).ready(function () {
    if (window.location.href.toLowerCase().includes("shippingdetails")) {
        let items = localStorage.getItem("shippingdata");
        if (items != null) {
            let data = JSON.parse(items);
            document.getElementById("cost").value = data.cost;
            document.getElementById("period").value = data.period;
            document.getElementById("state").value = data.state; 
            document.getElementById("totalprice").innerHTML = data.price;

            var coverage = $('#getdetails option:selected').text();
            if (coverage == "Within state") {
                $("#state").attr("readonly", true);
            }
        }

        $('#getdetails').on('change', function () {
            $.ajax({
                method: "GET",
                url: "/Market/ShippingJsonDetails",
                success: function (response) {
                    if (response != null) {
                        //console.log(response);
                    } else {
                        console.log("something went wrong");
                    }
                    var coverage = $('#getdetails option:selected').text();

                    if (coverage == "Within state") {
                        $("#state").val("Lagos");
                        $("#state").attr("readonly", true);
                        $('#cost').val(response[1].deliveryCost)
                        $('#period').val(response[1].deliveryPeriod)

                    }
                    if (coverage == "Outside state") {
                        $("#state").val("Select State");
                        $("#state").attr("readonly", false);

                        $('#cost').val(response[0].deliveryCost)
                        $('#period').val(response[0].deliveryPeriod)

                    }

                    if (coverage == "Select Coverage") {
                        $("#state").val("Select State");
                        $("#state").attr("readonly", false);

                        $('#cost').val("");
                        $('#period').val("");

                    }
                    let total = Number($('#hiddenPrice').val());
                    var t = total + Number($('#cost').val());
                    $('#totalprice').html(`${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(t)}`);
                }
            });

        });
    }




    $('#getdetails').on('change', function () {
        $.ajax({
            method: "GET",
            url: "/Market/ShippingJsonDetails",
            success: function (response) {
                if (response != null) {
                    //console.log(response);
                } else {
                    console.log("something went wrong");
                }
                var coverage = $('#getdetails option:selected').text();
                
                if (coverage == "Within state") {
                    $("#state").val("Lagos");
                    $("#state").attr("readonly", true);
                    $('#cost').val(response[1].deliveryCost)
                    $('#period').val(response[1].deliveryPeriod)
                    
                }
                if (coverage == "Outside state") {
                    $("#state").val("Select State");
                    $("#state").attr("readonly", false);

                    $('#cost').val(response[0].deliveryCost)
                    $('#period').val(response[0].deliveryPeriod)
                    
                }

                if (coverage == "Select Coverage") {
                    $("#state").val("Select State");
                    $("#state").attr("readonly", false);

                    $('#cost').val("");
                    $('#period').val("");

                }
                let total = Number($('#hiddenPrice').val());
                var t = total + Number($('#cost').val());
                $('#totalprice').html(`${new Intl.NumberFormat('en-NG', { style: 'currency', currency: 'NGN' }).format(t)}`);
            }


        });


        const button = document.querySelector("#create-agent__btn");
        button.addEventListener("click", function () {
            let cost = document.getElementById("cost").value;
            let period = document.getElementById("period").value;
            let state = document.getElementById("state").value;

            let price = document.getElementById("totalprice").innerHTML;

            let savingtolocalstorage = {
                cost: cost,
                period: period,
                state: state,
                price: price
            };
            console.log(savingtolocalstorage);
            localStorage.setItem('shippingdata', JSON.stringify(savingtolocalstorage));

        })

       


    })
});