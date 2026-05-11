// ===============================
// VALIDAR SENHAS
// ===============================
function validarSenhas() {
    const senha = document.getElementById("senha");
    const confirmarSenha = document.getElementById("confirmarSenha");

    if (senha.value !== confirmarSenha.value) {
        confirmarSenha.classList.add("is-invalid");
        return false;
    }

    confirmarSenha.classList.remove("is-invalid");
    return true;
}

// ===============================
// FILTRAR TABELA PEDIDOS
// ===============================
function filtrarTabela() {
    let input = document.getElementById("searchInput");
    if (!input) return;

    let filtro = input.value.toLowerCase();

    let abaAtiva = document.querySelector(".tab.active");
    let tab = abaAtiva ? abaAtiva.id.replace("tab-", "") : "todos";

    let linhas = document.querySelectorAll("#lista-pedidos tbody tr");

    linhas.forEach(linha => {
        let textoLinha = linha.textContent.toLowerCase();
        let status = linha.dataset.status;

        let correspondeBusca = textoLinha.includes(filtro);
        let correspondeStatus =
            tab === "todos" ||
            (tab === "pendentes" && status === "Pendente") ||
            (tab === "progresso" && status === "Em Produção") ||
            (tab === "concluidos" && status === "Concluído");

        linha.style.display = (correspondeBusca && correspondeStatus) ? "" : "none";
    });
}

// ===============================
// TROCAR ABAS
// ===============================
function switchTab(tab) {
    ["todos", "pendentes", "progresso", "concluidos"].forEach(nome => {
        const aba = document.querySelector(`#tab-${nome}`);
        if (aba) aba.classList.remove("active");
    });

    const abaAtiva = document.querySelector(`#tab-${tab}`);
    if (abaAtiva) abaAtiva.classList.add("active");

    const linhas = document.querySelectorAll("#lista-pedidos tbody tr");

    linhas.forEach(linha => {
        const status = linha.dataset.status;

        if (tab === "todos") {
            linha.style.display = "";
        }
        else if (tab === "pendentes" && status === "Pendente") {
            linha.style.display = "";
        }
        else if (tab === "progresso" && status === "Em Produção") {
            linha.style.display = "";
        }
        else if (tab === "concluidos" && status === "Concluído") {
            linha.style.display = "";
        }
        else {
            linha.style.display = "none";
        }
    });

    filtrarTabela();
}

// ===============================
// MODAL CANCELAR PEDIDO
// ===============================
const cancelarBtn = document.getElementById("cancelarPedido");
const modal = document.getElementById("confirmModal");
const fecharModal = document.getElementById("fecharModal");
const confirmarCancelamento = document.getElementById("confirmarCancelamento");

if (cancelarBtn && modal) {
    cancelarBtn.addEventListener("click", () => {
        modal.style.display = "flex";
    });
}

if (fecharModal && modal) {
    fecharModal.addEventListener("click", () => {
        modal.style.display = "none";
    });
}

if (confirmarCancelamento && modal) {
    confirmarCancelamento.addEventListener("click", () => {
        localStorage.removeItem("carrinho");
        modal.style.display = "none";
        carregarCarrinho();
        showToast("Pedido cancelado!");
    });
}

// ===============================
// MODAL FINALIZAR COMPRA
// ===============================
const finalizarCompra = document.getElementById("finalizarCompra");
const confirmarCompraModal = document.getElementById("confirmarCompraModal");
const fecharModalCompra = document.getElementById("fecharModalCompra");
const confirmarFinalizacao = document.getElementById("confirmarFinalizacao");

if (finalizarCompra && confirmarCompraModal) {
    finalizarCompra.addEventListener("click", () => {
        confirmarCompraModal.style.display = "flex";
    });
}

if (fecharModalCompra && confirmarCompraModal) {
    fecharModalCompra.addEventListener("click", () => {
        confirmarCompraModal.style.display = "none";
    });
}

// ===============================
// MODAL SALVAR ALTERAÇÕES
// ===============================
const abrirModalSalvar = document.getElementById("abrirModalSalvar");
const fecharModalSalvar = document.getElementById("fecharModalSalvar");
const modalSalvarAlteracoes = document.getElementById("modalSalvarAlteracoes");

if (abrirModalSalvar && modalSalvarAlteracoes) {
    abrirModalSalvar.addEventListener("click", () => {
        modalSalvarAlteracoes.style.display = "flex";
    });
}

if (fecharModalSalvar && modalSalvarAlteracoes) {
    fecharModalSalvar.addEventListener("click", () => {
        modalSalvarAlteracoes.style.display = "none";
    });
}

// ===============================
// MODAL EXCLUIR CONTA
// ===============================
const abrirModalExcluir = document.getElementById("abrirModalExcluir");
const fecharModalExcluir = document.getElementById("fecharModalExcluir");
const modalExcluirConta = document.getElementById("modalExcluirConta");

if (abrirModalExcluir && modalExcluirConta) {
    abrirModalExcluir.addEventListener("click", () => {
        modalExcluirConta.style.display = "flex";
    });
}

if (fecharModalExcluir && modalExcluirConta) {
    fecharModalExcluir.addEventListener("click", () => {
        modalExcluirConta.style.display = "none";
    });
}