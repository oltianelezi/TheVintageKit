 
      // Load cart from localStorage
      let cart = JSON.parse(localStorage.getItem("cart")) || [];

      // Global variable to store the product once fetched
      let currentProduct = null;

      // When page is ready run these
      $(document).ready(function () {
        renders(); // update cart icon
        loadProduct(); // load product from backend
      });

      function renders() {
        renderCartIcon(); // display cart icon or filled icon
      }

      // Listen for Add to Cart button click
      $(document).on("click", ".addToCart", function (event) {
        event.stopPropagation(); // stops click from triggering other events
        addToCart(); // adds product to cart
        $("#cartAlert").slideDown(); // show success alert popup
      });

      // Renders the shopping cart icon
      function renderCartIcon() {
        if (cart.length === 0) {
          $(".cartSymbol").replaceWith(
            `<div class="cartSymbol"><i class="bi-cart"></i></div>`
          ); //empty
        } else {
          $(".cartSymbol").replaceWith(
            `<div class="cartSymbol"><i class="bi-cart-fill"></i></div>`
          ); //filled
        }
      }

      // Fetch product details from backend API
      function loadProduct() {
        const params = new URLSearchParams(window.location.search);
        const productId = params.get("productId");

        if (!productId) {
          // No ID found → Display error
          $("#productDetails").html(`
            <div class="alert alert-danger text-center">
              <h4>Product not found</h4>
              <a href="products.html" class="btn btn-dark">Back to Products</a>
            </div>
          `);
          return;
        }

        // Frontend and Backend connects
        fetch(`http://localhost:5134/product/${productId}`)
          .then((response) => {
            if (!response.ok) throw new Error("Product not found");
            return response.json(); // convert backend response to JS object
          })
          .then((product) => {
            currentProduct = product; // store globally/ can be used in all the functions
            renderProduct(product); // build product UI dynamically
            document.title = `${product.productName} - The Vintage Kits`; // replace page title
          })
          .catch(() => {
            $("#productDetails").html(`
              <div class="alert alert-danger text-center">
                <h4>Product not found</h4>
                <a href="products.html" class="btn btn-dark">Back</a>
              </div>
            `);
          });
      }

      // Dynamically inserts product info into page
      function renderProduct(product) {
        const priceFormatted = product.price.toFixed(2); // format as 2 shifra pas presjes 0.00

        const productHtml = `

          <!-- LEFT SIDE: PRODUCT IMAGE -->

          <div class="row">
            <div class="col-md-6 mb-4">
              <div class="text-center">
                <img src="${product.imageURL}"
                     class="img-fluid rounded shadow"
                     style="max-height: 600px;">
              </div>
            </div>

            <!-- RIGHT SIDE: PRODUCT INFO -->

            <div class="col-md-6">
              <div class="bg-white rounded p-4 shadow">

                <h1 class="mb-3">${product.productName}</h1>
                <h2 class="text-success mb-4">$${priceFormatted}</h2>

                <!-- SIZE DROPDOWN -->

                <div class="mb-3">
                  <label class="form-label fw-bold">Size:</label>
                  <select class="form-select" id="size">
                    <option value="S">S - Small</option>
                    <option value="M" selected>M - Medium</option>
                    <option value="L">L - Large</option>
                    <option value="XL">XL - Extra Large</option>
                  </select>
                </div>


                <!-- QUANTITY SELECTOR -->

                <div class="mb-4">
                  <label class="form-label fw-bold">Quantity:</label>
                  <div class="input-group" style="width: 150px;">
                    <button class="btn btn-outline-secondary" id="decreaseBtn">-</button>
                    <input type="number" class="form-control text-center"
                           id="quantity" value="1" min="1" max="10">
                    <button class="btn btn-outline-secondary" id="increaseBtn">+</button>
                  </div>
                </div>

                <!-- ADD TO CART BUTTON -->

                <div class="d-grid gap-2 mb-3">
                  <button class="btn btn-dark btn-lg addToCart">
                    <i class="bi-cart me-2"></i>Add to Cart
                  </button>
                </div>

                <div class="mt-4">
                  <h5>Product Details</h5>
                  <ul class="list-unstyled">
                    <li><strong>Description:</strong> ${product.description}</li>
                    <li><strong>Fit:</strong> Regular fit</li>
                  </ul>
                </div>

              </div>
            </div>

          </div>
        `;

        $("#productDetails").html(productHtml); // insert HTML into element with id productDetails
        bindProductEvents(); // enable quantity buttons
      }

      // Attach events after HTML loads
      function bindProductEvents() {
        $("#increaseBtn").click(() => {
          $("#quantity").val(parseInt($("#quantity").val()) + 1); // increase quantity by 1 when you press +
        });

        $("#decreaseBtn").click(() => {
          const qty = parseInt($("#quantity").val());
          if (qty > 1) $("#quantity").val(qty - 1); // decrease quantity by 1 when you press - (if there is more than 1)
        });
      }

      // Adds product to cart array
      function addToCart() {
        if (!currentProduct) return;

        cart.push({
          productId: currentProduct.productID,
          quantity: parseInt($("#quantity").val()),
          size: $("#size").val(),
          productName: currentProduct.productName,
          price: currentProduct.price,
          imageURL: currentProduct.imageURL,
        });

        localStorage.setItem("cart", JSON.stringify(cart));
        renders(); // update cart icon
      }
