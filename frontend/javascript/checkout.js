
      const checkoutReceipt = JSON.parse(
        localStorage.getItem("checkoutReceipt")
      );

      if (!checkoutReceipt || checkoutReceipt.checkoutPrice === 0) {
        window.location.replace("homepage.html");
      }

      function getAddress() {
        return {
          firstName: $("#firstName").val().trim(),
          lastName: $("#lastName").val().trim(),
          street: $("#street").val().trim(),
          city: $("#city").val().trim(),
          state: $("#state").val().trim(),
          zip: $("#zip").val().trim(),
          country: $("#country").val().trim(),
        };
      }

      function checkAddressFields() {
        const address = getAddress();
        const emptyField = Object.entries(address).find(
          ([key, value]) => value === ""
        );
        if (emptyField) {
          $("#paypal-overlay").show();
        } else {
          $("#paypal-overlay").hide();
        }
      }

      $(document).ready(function () {
        // Initial overlay check
        checkAddressFields();

        // Validate on input
        $("#firstName, #lastName, #street, #city, #state, #zip, #country").on(
          "input",
          checkAddressFields
        );

        // Render PayPal button
        paypal
          .Buttons({
            createOrder: function (data, actions) {
              return actions.order.create({
                purchase_units: [
                  {
                    amount: { value: checkoutReceipt.checkoutPrice.toFixed(2) },
                  },
                ],
              });
            },
            onApprove: function (data, actions) {
              const address = getAddress();
              const emptyField = Object.entries(address).find(
                ([key, value]) => value === ""
              );
              if (emptyField) {
                alert(
                  "Please fill in all shipping address fields before completing payment."
                );
                return;
              }

              return actions.order.capture().then(function (details) {
                const payload = {
                  paymentAmount: checkoutReceipt.checkoutPrice,
                  address,
                  order: checkoutReceipt.order,
                  email: details.payer.email_address,
                };
                console.log(payload);

                // alert("Payment completed by " + details.payer.name.given_name);

                fetch("http://localhost:5134/order", {
                  method: "POST",
                  headers: { "Content-Type": "application/json" },
                  body: JSON.stringify(payload),
                })
                  .then((res) =>
                    res.ok
                      ? $(document).ready(function () {
                          $("#paymentAlert").slideDown();
                          $("#paypal-overlay").show();

                          $(document).on("click", "#alertRemove", function () {
                            $("#paymentAlert").slideUp();
                            window.location.replace("homepage.html");
                          });
                        })
                      : console.error("Backend error")
                  )
                  .catch((err) => console.error(err));

                localStorage.removeItem("checkoutReceipt");
                localStorage.removeItem("cart");
              });
            },
          })
          .render("#paypal-button-container");
      });
    