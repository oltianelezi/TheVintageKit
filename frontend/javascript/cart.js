
      // Get the cart from localStorage
      let cart = JSON.parse(localStorage.getItem("cart")) || [];
      let products;

      async function loadProducts() {
        products = await Promise.all(
          cart.map((item) =>
            fetch(`http://localhost:5134/product/${item.productId}`).then(
              (res) => res.json()
            )
          )
        );
      }

      $(document).ready(async function () {
        await loadProducts();
        renders();
        // Get all product info and map them to correct cart index to preserve order
        // Handle size changes
        $(document).on("change", ".sizeSelect", function () {
          const index = $(this).data("id");
          const newSize = $(this).val();

          cart[index].size = newSize;
          localStorage.setItem("cart", JSON.stringify(cart));
        });

        // Handle quantity changes
        $(document).on("input", ".quantityInput", function () {
          const index = $(this).data("id");
          const newQuantity = parseInt($(this).val(), 10);

          if (!isNaN(newQuantity) && newQuantity > 0) {
            cart[index].quantity = newQuantity;
            localStorage.setItem("cart", JSON.stringify(cart));
            renders();
          }
        });
      });

      // Render changing DOM elements
      function renders() {
        renderCartIcon();
        renderTable();
      }

      // Renders the cart table + total price row
      async function renderTable() {
        const productTable = $(".productTable"); // the table container
        productTable.find("tbody").empty();

        let totPrice = 0;

        if (cart.length === 0) {
          // If cart is empty, replace the table with an alert
          productTable.html(`
                    <div class="container mt-3">
                      <div class="alert alert-danger">
                        There is nothing in your cart
                      </div>
                    </div>
                  `);
        } else {
          console.log(products);

          products.forEach((product, index) => {
            const item = cart[index];
            totPrice += product.price * item.quantity;
            totPrice = Math.round(totPrice * 100) / 100;
            console.log(item);

            productTable.find("tbody").append(`
                      <tr>
                        <td>
                          <button type="button" class="btn btn-dark removeItem" data-id="${index}" data-price="${
              product.price
            }">
                            <i class="bi-x-lg"></i>
                          </button>
                          <img src="${
                            product.imageURL
                          }" style="height: 100px; margin-left: 50px" />
                          ${product.productName}
                        </td>
                        <td>
                          <select class="form-select bg-dark text-white mt-4 sizeSelect" data-id="${index}">
                            <option value="S" ${
                              item.size === "S" ? "selected" : ""
                            }>S</option>
                            <option value="M" ${
                              item.size === "M" ? "selected" : ""
                            }>M</option>
                            <option value="L" ${
                              item.size === "L" ? "selected" : ""
                            }>L</option>
                            <option value="XL" ${
                              item.size === "XL" ? "selected" : ""
                            }>XL</option>
                          </select>
                        </td>
                        <td>
                          <input type="number" class="form-control bg-dark text-white mt-4 w-25 quantityInput"
                          value="${item.quantity}" data-id="${index}"/>
                        </td>
                        <td><p class="mt-4">$${product.price}</p></td>
                      </tr>
                    `);
          });

          // Total price row
          productTable.find("tbody").append(`
                  <tr class="totalRow">
                    <td colspan="3" class="text-end fw-bold">Total:</td>
                    <td class="fw-bold">$${totPrice}</td>
                  </tr>
                `);
        }
      }

      // Remove item handler
      $(document).on("click", ".removeItem", function () {
        const removeIndex = $(this).data("id");

        cart.splice(removeIndex, 1);
        products.splice(removeIndex, 1);
        localStorage.setItem("cart", JSON.stringify(cart));

        renders();
      });

      // Renders the cart icon filled or empty
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

      function handleCheckout() {
        let order = [];
        for (let i = 0; i < cart.length; i++) {
          order.push({
            productId: products[i].productID,
            size: cart[i].size,
            quantity: cart[i].quantity,
            unitPrice: products[i].price * cart[i].quantity,
          });
        }
        

        let checkoutPrice = 0;
        for (let i = 0; i < cart.length; i++) {
          checkoutPrice += products[i].price * cart[i].quantity;
        }

        let checkoutReceipt = { order: order, checkoutPrice: checkoutPrice };
        localStorage.setItem(
          "checkoutReceipt",
          JSON.stringify(checkoutReceipt)
        );
      }

      $(document).on("click", ".checkoutBtn", handleCheckout);
    