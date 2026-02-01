
      // Load cart or start empty
      let cart = JSON.parse(localStorage.getItem("cart")) || [];

      $(document).ready(function () {
        renders();

        $("#search").on("keyup", function () {
          var value = $(this).val().toLowerCase();
          $("#productSection .card-body").filter(function () {
            $(this)
              .parent()
              .toggle($(this).text().toLowerCase().indexOf(value) > -1);
          });
        });

        $("#searchButton").click(function () {
          $("#searchBar").slideToggle();
        });
      });

      function renders() {
        renderCartIcon();
      }

      function renderCartIcon() {
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

      $(document).on("click", ".productCard", goToProduct);

      function goToProduct() {
        const productId = $(this).data("id");
        window.location.href = `single-product.html?productId=${productId}`;
      }

      // Add item to cart
      function addToCart(productId) {
        const product = cart.find((item) => item.productId === productId);

        cart.push({ productId, quantity: 1, size: "M" });
        localStorage.setItem("cart", JSON.stringify(cart));
        renders();
      }

      $(document).on("click", ".addToCart", function (event) {
        event.stopPropagation(); // This prevents the card click event from firing

        const productId = $(this).data("id");
        addToCart(productId);
        $("#cartAlert").slideDown();

        $(document).on("click", "#alertRemove", function () {
          $("#cartAlert").slideUp();
        });
      });


      $(document).ready(function () {
        const params = new URLSearchParams(window.location.search);
        const league = params.get("league");
        // Fetch products
        const apiEndpoint = league
          ? `http://localhost:5134/product/league/${league}`
          : "http://localhost:5134/product";
        fetch(apiEndpoint)
          .then((response) => response.json()) // parse JSON once
          .then((products) => {
            console.log(products); // now you have the array

            // Clear old cards
            $("#productsRow").empty();

            // Render cards dynamically
            products.forEach((product) => {
              const priceFormatted = product.price.toFixed(2); // 2 decimals
              const card = `
                    <div class="col" >
                        <div class="card m-3 productCard" data-id="${product.productID}" style="width: 18rem; height: 29rem; cursor: pointer;">
                            <img src="${product.imageURL}" class="card-img-top" alt="${product.productName}">
                            <div class="card-body">
                                <h5 class="card-title">${product.productName}</h5>
                                <p class="card-text">$${priceFormatted}</p>
                                <button class="btn btn-dark text-white addToCart" data-id="${product.productID}">
                                    Add to cart <i class="bi-cart"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                `;
              $("#productsRow").append(card);
            });
          })
          .catch((error) => {
            console.error("Error fetching products:", error);
          });
      });
    
