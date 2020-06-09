function get_dados_salvar() {
    return {
        Id: $('#cadastro_id').val(),
        Nome: $('#txt_nome').val(),
        Sigla: $('#txt_sigla').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function set_dados_form(dados) {
    $('#cadastro_id').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_sigla').val(dados.Sigla);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Sigla + '</td>' +
        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Sigla: '',
        Ativo: true
    };
}

function incluir_linha_grid_salvo(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Sigla).end()
        .eq(2).html(param.Ativo ? 'Sim' : 'Não');
}
