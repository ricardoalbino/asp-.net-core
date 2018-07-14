

//----
function CarregarUsuarioLogado() {

    var nome = '@HttpContextAccessor.HttpContext.Session.GetString("NomeUsuarioLogado")';
    var divnome = document.getElementsByName("login");

    alert(nome);

}