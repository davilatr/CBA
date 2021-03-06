﻿function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatar_msg_avisos(mensagens) {
    var ret = '';

    for (var i = 0; i < mensagens.length; i++)
        ret += '<li>' + mensagens[i] + '</li>';
    return '<ul>' + ret + '</ul>';
}

function abrir_modal(dados) {
    set_dados_form(dados);

    var modalCadastro = $('#modal_cadastro');

    $('#aviso').empty();
    $('#aviso').hide();
    $('#msg_erro').hide();
    $('#msg_aviso').hide();

    bootbox.dialog({
        title: tituloModal,
        message: modalCadastro
    })
        .on('shown.bs.modal', function () {
            modalCadastro.show(0, function () {
                set_focus_form();
            });
        })
        .on('hidden.bs.modal',
            function () {
                modalCadastro.hide().appendTo('body');
            });
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        set_dados_grid(dados) +
        '<td>' +
        '<a id="btn_alterar" class="btn btn-primary" role="button" style="margin-right: 3px"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Alterar</a>' +
        '<a id="btn_excluir" class="btn btn-danger" role="button"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span> Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}

$(document)
    .on('click', '#btn_incluir', function () {
        abrir_modal(get_dados_inclusao());
    })
    .on('click', '#btn_alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_alterar,
            param = { 'id': id };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                if (url_alterar.includes('Usuario/ListaUsuario'))
                    if (_senhaPadrao)
                        response.Senha = _senhaPadrao;

                abrir_modal(response);
            }
        })
        .fail(function () {
            Swal.fire('Aviso',
                'Não foi possível recuperar as informações no momento. Tente novamente mais tarde!',
                'warning');
        });
    })
    .on('click', '#btn_excluir', function () {
        var
            btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            item = tr.find('td:first-child').text(),
            url = url_excluir,
            param = { 'id': id };


        bootbox.confirm({
            message: ('Realmente deseja excluir o item: <b>' + item + '</b> ?'),
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {
                    //console.log('This was logged in the callback: ' + result);
                    $.post(url, add_anti_forgery_token(param), function (response) {
                        if (response) {
                            tr.remove();
                            var count_grid = $('#grid_cadastro > tbody > tr').length;
                            if (count_grid == 0) {
                                $('#grid_cadastro').addClass('collapse');
                                $('#qtdeMaxLinha').addClass('collapse');
                                $('#grid_semRegistro').removeClass('collapse');
                            }
                        }
                    })
                    .fail(function () {
                        Swal.fire('Aviso',
                            'Não foi possível excluir o item. Tente novamente mais tarde!',
                            'warning');
                    });
                }
            }
        });
    })
    .on('click', '#btn_salvar', function () {
        var btn = $(this),
            url = url_salvar,
            param = get_dados_salvar();

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == 'ok') {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);

                    table.append(linha);
                    $('#grid_cadastro').removeClass('collapse');
                    $('#qtdeMaxLinha').removeClass('collapse');
                    $('#grid_semRegistro').addClass('collapse');
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    incluir_linha_grid_salvo(param, linha);
                }

                $('#modal_cadastro').parents('.bootbox').modal('hide');
            }
            else if (response.Resultado == 'erro') {
                $('#msg_erro').show();
                $('#msg_aviso').hide();
                $('#aviso').hide();
            }
            else if (response.Resultado == 'aviso') {
                $('#aviso').html(formatar_msg_avisos(response.Mensagens));
                $('#msg_erro').hide();
                $('#msg_aviso').show();
                $('#aviso').show();
            }
        })
        .fail(function () {
            Swal.fire('Aviso',
                'Não foi possível salvar o item no momento. Tente novamente mais tarde!',
                'warning');
        });
    })
    .on('click', '.num_pag', function () {
        var btn = $(this),
            tamPag = $('#ddl_qtdeMaxLinhasPorPagina').val(),
            filtro = $('#txt_filtro'),
            pagina = btn.text(),
            url = url_paginacao_click,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();

                if (response.length > 0) {
                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }

                    $('#grid_cadastro').removeClass('collapse');
                    $('#qtdeMaxLinha').removeClass('collapse');
                    $('#grid_semRegistro').addClass('collapse');
                }
                else {
                    $('#grid_cadastro').addClass('collapse');
                    $('#qtdeMaxLinha').addClass('collapse');
                    $('#grid_semRegistro').removeClass('collapse');
                }

                btn.siblings().removeClass('active');
                btn.addClass('active');
            }
        })
        .fail(function () {
            Swal.fire('Aviso',
                'Não foi possível recuperar as informações no momento. Tente novamente mais tarde!',
                'warning');
        });
    })
    .on('change', '#ddl_qtdeMaxLinhasPorPagina', function () {
        var ddl = $(this),
            tamPag = ddl.val(),
            filtro = $('#txt_filtro'),
            pagina = 1,
            url = url_tam_paginacao_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                if (response.length > 0) {
                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }

                    $('#grid_cadastro').removeClass('collapse');
                    $('#qtdeMaxLinha').removeClass('collapse');
                    $('#grid_semRegistro').addClass('collapse');
                }
                else {
                    $('#grid_cadastro').addClass('collapse');
                    $('#qtdeMaxLinha').addClass('collapse');
                    $('#grid_semRegistro').removeClass('collapse');
                }

                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }
        })
        .fail(function () {
            Swal.fire('Aviso',
                'Não foi possível recuperar as informações no momento. Tente novamente mais tarde!',
                'warning');
        });
    })
    .on('keyup', '#txt_filtro', function () {
        var filtro = $(this),
            ddl = $('#ddl_qtdeMaxLinhasPorPagina'),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_tam_paginacao_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();

                if (response.length > 0) {
                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }

                    $('#grid_cadastro').removeClass('collapse');
                    $('#qtdeMaxLinha').removeClass('collapse');
                    $('#grid_semRegistro').addClass('collapse');
                }
                else {
                    $('#grid_cadastro').addClass('collapse');
                    $('#qtdeMaxLinha').addClass('collapse');
                    $('#grid_semRegistro').removeClass('collapse');
                }


                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }
        })
        .fail(function () {
            Swal.fire('Aviso',
                'Não foi possível recuperar as informações no momento. Tente novamente mais tarde!',
                'warning');
        });
    });