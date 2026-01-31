
      function cartIconRender() {
        let cart = JSON.parse(localStorage.getItem("cart")) || [];

        if (cart.length === 0) {
          $(".cartSymbol").replaceWith(
            `<div class="cartSymbol"><i class="bi-cart"></i></div>`
          );
        } else {
          $(".cartSymbol").replaceWith(
            `<div class="cartSymbol"><i class="bi-cart-fill"></i></div>`
          );
        }
      }

      $(document).ready(function () {
        cartIconRender();
      });

      async function submitMessage() {
        console.log("click");
        
        const payload = {
          name: $("#name").val(),
          email: $("#email").val(),
          comment: $("#comment").val(),
        };

        const response = await fetch(
          "http://localhost:5134/message/sendMessage",
          {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
          }
        );

        if (response.ok) {
          $("#confirmationAlert").slideDown();

          $(document).on("click", "#alertRemove", function () {
            $("#confirmationAlert").slideUp();
            window.location.replace("homepage.html");
          });
        }
      }

      $(document).on("click", "#submitBtn", function (e) {
        e.preventDefault();
        submitMessage();
      });
 