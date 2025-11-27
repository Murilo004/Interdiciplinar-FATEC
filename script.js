function showToast() {
  const toast = document.getElementById("toast");
  toast.classList.add("show");

  setTimeout(() => {
    toast.classList.remove("show");
  }, 3000);
}

// Barra de Pesquisa
function filtrarTabela() {
    let input = document.getElementById("searchInput");
    let filtro = input.value.toLowerCase();

    // detecta qual tabela está visível
    let tabelas = document.querySelectorAll(".pedido-lista");
    let tabelaVisivel = null;

    tabelas.forEach(tbl => {
        if (tbl.style.display !== "none") {
            tabelaVisivel = tbl;
        }
    });

    if (!tabelaVisivel) return;

    let linhas = tabelaVisivel.getElementsByTagName("tr");

    for (let i = 1; i < linhas.length; i++) {  
        let textoLinha = linhas[i].textContent.toLowerCase();

        if (textoLinha.includes(filtro)) {
            linhas[i].style.display = "";
        } else {
            linhas[i].style.display = "none";
        }
    }
}


function gerarListas() {
  const tabelaTodos = document.querySelector("#lista-todos");
  const linhas = tabelaTodos.querySelectorAll("tr");
  
  // Cabeçalho padrão
  const header = `
    <tr>
      <th>Pedido ID</th>
      <th>Data</th>
      <th>Cliente</th>
      <th>Status</th>
      <th>Serviços</th>
    </tr>`;

  // Limpa tabelas
  document.querySelector("#lista-progresso").innerHTML = header;
  document.querySelector("#lista-concluidos").innerHTML = header;
  document.querySelector("#lista-enviados").innerHTML = header;

  // Percorre linhas (pula cabeçalho)
  for (let i = 1; i < linhas.length; i++) {
    let linha = linhas[i].cloneNode(true);
    let status = linha.querySelector(".status-badge").textContent.trim();

    if (status === "Em Produção") {
      document.querySelector("#lista-progresso").appendChild(linha);
    } 
    else if (status === "Concluído") {
      document.querySelector("#lista-concluidos").appendChild(linha);
    }
    else if (status === "Enviado") {
      document.querySelector("#lista-enviados").appendChild(linha);
    }
  }
}

function switchTab(tab) {
  // Oculta todas
  document.querySelector("#lista-todos").style.display = "none";
  document.querySelector("#lista-progresso").style.display = "none";
  document.querySelector("#lista-concluidos").style.display = "none";
  document.querySelector("#lista-enviados").style.display = "none";

  // Remove active
  document.querySelectorAll(".tab").forEach(t => t.classList.remove("active"));

  // Mostra a escolhida
  document.querySelector("#lista-" + tab).style.display = "table";
  document.querySelector("#tab-" + tab).classList.add("active");
}

// Gera as tabelas ao carregar a página
window.onload = gerarListas;


function showToast(message) {
    const toast = document.getElementById('toast');

    toast.textContent = message;
    toast.classList.add('show');

    setTimeout(() => {
        toast.classList.remove('show');
    }, 3000);
}

function showRemovedToast() {
    showToast("Pedido removido!");
}


// ------------------------------
// ADICIONAR ITEM AO CARRINHO (Página Serviços)
// ------------------------------
document.addEventListener("click", function(e) {
    if (e.target.classList.contains("add-carrinho")) {
        e.preventDefault();

        const nome = e.target.dataset.nome;
        const preco = Number(e.target.dataset.preco);

        let carrinho = JSON.parse(localStorage.getItem("carrinho")) || [];

        carrinho.push({ nome, preco });

        localStorage.setItem("carrinho", JSON.stringify(carrinho));

        showToast("Item adicionado ao carrinho!");
    }
});


// ------------------------------
// EXIBIR ITENS NA PÁGINA CARRINHO
// ------------------------------
function carregarCarrinho() {
    const tabela = document.getElementById("carrinho-body");
    const totalSpan = document.getElementById("totalValor");

    if (!tabela) return; // Se não estiver no Carrinho.html, não executa nada

    tabela.innerHTML = "";

    let carrinho = JSON.parse(localStorage.getItem("carrinho")) || [];

    carrinho.forEach((item, index) => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${item.nome}</td>
            <td>R$ ${item.preco.toFixed(2)}</td>
            <td><button class="remover" data-index="${index}">Remover</button></td>
        `;
        tabela.appendChild(tr);
    });

    const total = carrinho.reduce((sum, i) => sum + i.preco, 0);
    totalSpan.textContent = total.toFixed(2);
}

carregarCarrinho();


// ------------------------------
// REMOVER ITEM DO CARRINHO
// ------------------------------
document.addEventListener("click", function(e) {
    if (e.target.classList.contains("remover")) {

        const index = e.target.dataset.index;

        let carrinho = JSON.parse(localStorage.getItem("carrinho")) || [];
        carrinho.splice(index, 1);

        localStorage.setItem("carrinho", JSON.stringify(carrinho));

        carregarCarrinho();
        
        showRemovedToast();
    }
});


// --------------------------------------------
// MODAL — CANCELAR PEDIDO (CONFIRMAÇÃO)
// --------------------------------------------
const cancelarBtn = document.getElementById("cancelarPedido");
const modal = document.getElementById("confirmModal");
const fecharModal = document.getElementById("fecharModal");
const confirmarCancelamento = document.getElementById("confirmarCancelamento");

// Abrir modal
if (cancelarBtn) {
    cancelarBtn.addEventListener("click", () => {
        modal.style.display = "flex";
    });
}

// Fechar modal
if (fecharModal) {
    fecharModal.addEventListener("click", () => {
        modal.style.display = "none";
    });
}

// Cancelar pedido completamente
if (confirmarCancelamento) {
    confirmarCancelamento.addEventListener("click", () => {
        localStorage.removeItem("carrinho");
        modal.style.display = "none";
        carregarCarrinho(); // Atualiza a tabela
        showToast("Pedido cancelado!");
    });
}

// BOTÃO FINALIZAR COMPRA
document.getElementById("finalizarCompra").addEventListener("click", () => {
    document.getElementById("confirmarCompraModal").style.display = "flex";
});

// FECHAR MODAL (botão "Não")
document.getElementById("fecharModalCompra").addEventListener("click", () => {
    document.getElementById("confirmarCompraModal").style.display = "none";
});

// CONFIRMAR FINALIZAÇÃO
document.getElementById("confirmarFinalizacao").addEventListener("click", () => {

    // ==============================
    // 1️⃣ LIMPA OS DADOS DO LOCALSTORAGE
    // ==============================
    localStorage.removeItem("carrinho");  

    // ==============================
    // 2️⃣ REMOVE ITENS DA TABELA NA TELA
    // ==============================
    const tbody = document.getElementById("carrinho-body");
    tbody.innerHTML = "";

    // Atualiza o total para zero
    document.getElementById("totalValor").innerText = "0.00";

    // ==============================
    // 3️⃣ MOSTRA O TOAST
    // ==============================
    showToast("Compra finalizada com sucesso!");

    // ==============================
    // 4️⃣ RECARREGA A PÁGINA DEPOIS DE 1.2s
    // ==============================
    setTimeout(() => {
        location.reload();
    }, 1500);
});