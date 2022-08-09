

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();
//console.log(connection);

$("#sendBtn").click(function () {
    let message = $("#textArea").val();
    connection.invoke("SendMessage", message)
    
})
connection.on("ReceiveMessage", function (message) {
    let li = `<li class="d-flex justify-content-between mb-4">
                  <div class="card mask-custom w-100">
                        <div class="card-header d-flex justify-content-between p-3"
                          style="border-bottom: 1px solid rgba(255,255,255,.3);">
                          <p class="fw-bold mb-0">Lara Croft</p>
                          <p class="text-light small mb-0"><i class="far fa-clock"></i> 13 mins ago</p>
                        </div>
                        <div class="card-body">
                          <p class="mb-0">
                            ${message}
                          </p>
                        </div>
                      </div>
                      <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-5.webp" alt="avatar"
                        class="rounded-circle d-flex align-self-start ms-3 shadow-1-strong" width="60">
              </li>`

    //$(`<li class="d-flex justify-content-between mb-4">
    //              <div class="card mask-custom w-100">
    //                <div class="card-header d-flex justify-content-between p-3"
    //                  style="border-bottom: 1px solid rgba(255,255,255,.3);">
    //                  <p class="fw-bold mb-0">Lara Croft</p>
    //                 <p class="text-light small mb-0"><i class="far fa-clock"></i> 13 mins ago</p>
    //                </div>
    //              <div class="card-body">
    //                 <p class="mb-0">
    //                   ${message}
    //                 </p>
    //               </div>
    //             </div>
    //             <img src="https://mdbcdn.b-cdn.net/img/Photos/Avatars/avatar-5.webp" alt="avatar"
    //                class="rounded-circle d-flex align-self-start ms-3 shadow-1-strong" width="60">
    //            </li>`).appendTo($("#messagesList"))
    $("#messagesList").append(li);
    $(".messageBox").scrollTop(10000)
    $("#textArea").val(``);
});
connection.on("UserConnect", function (userId) {
    $(`#${userId}`).removeClass("d-none");
});
connection.on("UserDisconnect", function (userId) {
    $(`#${userId}`).addClass("d-none");
});

