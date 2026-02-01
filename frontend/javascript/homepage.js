
      // Load cart or start empty
      let cart = JSON.parse(localStorage.getItem("cart")) || [];
      //it turns it from json to js from the local storage
      $(document).ready(function () {
        renders();
      });

      $(document).on("click", ".productCard", goToProduct);

      function goToProduct() {
        //reads the id attribute from the clicked
        const productId = $(this).data("id");
        window.location.href = `single-product.html?productId=${productId}`;
      }
      //destination = bestsellersrow
      function loadProducts(products, destination) {
        for (const id of products) {
          fetch(`http://localhost:5134/product/${id}`) // change onrender url to "http://localhost:5134" for local deployment
            .then((response) => response.json()) // parse JSON once
            .then((product) => {
              console.log(product);
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
              $(destination).append(card);
            })
            .catch((error) => {
              console.error("Error fetching products:", error);
            });
        }
      }

      function renders() {
        cartIconRender();
      }

      function cartIconRender() {
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
    
      
        //Wait until html loaded then function starts
        $(document).ready(function () {
          const bestSellers = [10, 11, 25];

          loadProducts(bestSellers, "#bestSellersRow");
        });
      
         
        $(document).ready(function () {
          const worldCupClassics = [14, 21, 8];
          loadProducts(worldCupClassics, "#worldCupClassicsRow");
        });
      