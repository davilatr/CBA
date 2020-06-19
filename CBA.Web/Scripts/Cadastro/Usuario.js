function set_dados_form(dados) {
    $('#cadastro_id').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Login + '</td>';
}

function get_dados_salvar() {
    return {
        Id: $('#cadastro_id').val(),
        Nome: $('#txt_nome').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val()
    };
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Login: '',
        Senha: ''
    };
}

function incluir_linha_grid_salvo(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Login).end();
}
