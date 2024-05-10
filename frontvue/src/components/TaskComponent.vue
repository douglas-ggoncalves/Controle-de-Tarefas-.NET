<template>
  <section id="homePage">
    <div id="content">
      <div id="subMain">
        <div id="spacer"/>

        <div id="leftContent">
          <div>
            <h4>Filtros</h4>
            <button id="allCategories" class="active" @click="filterTasks('all'); activeClass('allCategories')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Todas
            </button>

            <button id="onlyUrgent" class="roundedUrgent" @click="filterTasks('urgent'); activeClass('onlyUrgent')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Urgentes<!-- <button>{{ itemsDataTable | totalUrgent }}</button>  -->
            </button>

            <button id="onlyImportant" class="roundedImportant" @click="filterTasks('important'); activeClass('onlyImportant')"> 
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Importantes<!-- <button>{{ itemsDataTable | totalImportant }}</button>-->
            </button>

            <button id="onlyOthers" class="roundedOthers" @click="filterTasks('others'); activeClass('onlyOthers')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Outras<!-- <button>{{ itemsDataTable | totalOthers }}</button>-->
            </button>

            <button id="onlyInPending" class="roundedOthers" @click="filterTasks('inPending'); activeClass('onlyInPending')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Pendente
            </button>

            <button id="onlyInProgress" @click="filterTasks('inProgress'); activeClass('onlyInProgress')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Em andamento
            </button>

            <button id="onlyFinished" @click="filterTasks('finished'); activeClass('onlyFinished')">
              <font-awesome-icon icon="fa-solid fa-chevron-right" />
              Concluídas
            </button>
          </div>
        </div>

        <div id="rightContent">
          <div>
            <h3>Minhas Tarefas</h3>
            
            <div class="input-container" @click="inputFocus()">
              <input id="searchTaskInput" class="input-field" type="text" v-model="searchTable" placeholder="Buscar Tarefas">
              <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
            </div>

            <v-data-table :no-data-text="'Não há dados'" :no-results-text="'Nenhum resultado encontrado'" 
            :header-props="{'sortByText': 'Ordenar por'}" :footer-props="{'items-per-page-text':'Itens por página', 
            pageText: '{0}-{1} de {2}', 'items-per-page-all-text':'Todos'}" :headers="headersDataTable" :items="itemsFilteredDataTable" 
            :search="searchTable" :custom-filter="filterDataDataTable">
            <template v-slot:[`item.titulo`]="{ item }">
                <v-tooltip :color="'rgb(0, 0, 0)'" :max-width="220" bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <span v-bind="attrs" v-on="on" style="display: inline-block; max-width: 220px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">{{ item.titulo }}</span>
                  </template> 
                  <span>{{ item.titulo }}</span>
                </v-tooltip>
              </template>

              <template v-slot:[`item.descricao`]="{ item }">
                <v-tooltip class="p-0" :color="'rgb(0, 0, 0)'" :max-width="220" bottom>
                  <template v-slot:activator="{ on, attrs }">
                    <span v-bind="attrs" v-on="on" style="display: inline-block; max-width: 220px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">{{ item.descricao }}</span>
                  </template> 
                  <span>{{ item.descricao }}</span>
                </v-tooltip>
              </template>

              <template v-slot:[`item.codigoStatus`]="{ item }">
                <span style="min-width: 100px; display: inline-block;">{{ item.codigoStatus | toDescriptionDetail }}</span>
              </template>

              <template v-slot:[`item.codigoCategoria`]="{ item }">
                <span style="min-width: 115px; display: inline-block;">{{ item.codigoCategoria | toDescriptionCategoryDetail }}</span>
              </template>

              <template v-slot:[`item.dataInicial`]="{ item }">
                <span style="min-width: 100px; display: inline-block;">{{ item.dataInicial | toDate }}</span>
              </template>

              <template v-slot:[`item.dataFinal`]="{ item }">
                <span style="min-width: 100px; display: inline-block;">{{ item.dataFinal | toDate }}</span>
              </template>

              <template v-slot:[`item.action`]="{ item }"> 
                <v-icon small class="mr-2" @click="editItem(item)">
                  mdi-pencil
                </v-icon>
                <v-icon small @click="deleteItem(item)">
                  mdi-delete
                </v-icon>
              </template>
            </v-data-table>
          </div>
        </div>
        
        <button id="addTask" @click="newTask()">
          <font-awesome-icon icon="fa-solid fa-plus" />
        </button>
      </div>
    </div>
    
    <div id="myModal" class="modal">
      <div class="modal-content">
        <div>
          <span><h3>Cadastrar Tarefa</h3></span>
          <span class="close" style="margin-bottom: auto; line-height: 1" @click="clearNewTaskData()">&times;</span>
        </div>

        <div>
          <form action="">
            <span>Título:</span>
            <input v-model="title" type="text">

            <span>Descrição:</span>
            <textarea v-model="description" name="" id="" cols="30" rows="10" spellcheck="false"></textarea>
            <v-menu ref="menuDataInicial" v-model="menuDataInicial" :close-on-content-click="false" transition="scale-transition" offset-y max-width="290px" min-width="auto">
              <template v-slot:activator="{ on, attrs }">
                <v-text-field v-model="dateMovInicial" readonly label="Data Inicial *" hint="informe a data desejada" v-bind="attrs" @blur="dateDataInicial = parseDate(dateMovInicial)" v-on="on" ></v-text-field>
              </template>
                
              <v-date-picker v-model="dateDataInicial" no-title @input="menuDataInicial = false" locale="pt"></v-date-picker>
            </v-menu>

            <v-menu ref="menuDataFinal" v-model="menuDataFinal" :close-on-content-click="false" transition="scale-transition" offset-y max-width="290px" min-width="auto">
              <template v-slot:activator="{ on, attrs }">
                <v-text-field v-model="dateMovFinal" readonly label="Data Final *" hint="informe a data desejada" v-bind="attrs" @blur="dateDataFinal = parseDate(dateMovFinal)" v-on="on" ></v-text-field>
              </template>
                
              <v-date-picker v-model="dateDataFinal" no-title @input="menuDataFinal = false" locale="pt"></v-date-picker>
            </v-menu>
          </form>
        </div>

        <span class="titleS">Status:</span>
        <div class="modalStatusFooter">

          <span @click="changeStatusEdit(0)">
            <button id="statusPending">
              
            </button>
            Pendente
          </span>

          <span @click="changeStatusEdit(2)">
            <button id="statusInProgress">
              
            </button>
            Em andamento
          </span>
          
          <span @click="changeStatusEdit(1)">
            <button id="statusCompleted">
              
            </button>
            Concluída
          </span>
        </div>

        <span class="titleS">Categoria:</span>
        <div class="modalFooter">
          <span @click="changeCategory('urgent')">
            <button id="categoryUrgent">
              
            </button>
            Urgente
          </span>
          
          <span @click="changeCategory('important')">
            <button id="categoryImportant">
              
            </button>
            Importante
          </span>
          
          <span @click="changeCategory('others')">
            <button id="categoryOthers">
              
            </button>
            Outras
          </span>

          <button id="add" @click="addTask()">
            Adicionar
          </button>
        </div>
      </div>
    </div>

    <div id="modalEdit" class="modal">
      <div class="modal-content">
        <div>
          <span><h3>Editar Tarefa</h3></span>
          <span class="close" style="margin-bottom: auto; line-height: 1" @click="clearEditTaskData()">&times;</span>
        </div>

        <div>
          <form action="">
            <span>Título:</span>
            <input v-model="modalEdit.titulo" type="text">

            <span>Descrição:</span>
            <textarea v-model="modalEdit.descricao" name="" id="" cols="30" rows="10" spellcheck="false"></textarea>
          </form>
        </div>
        
        <v-menu ref="menuEditDataInicial" v-model="menuEditDataInicial" :close-on-content-click="false" transition="scale-transition" offset-y max-width="290px" min-width="auto">
          <template v-slot:activator="{ on, attrs }">
            <v-text-field v-model="dateMovInicial" readonly label="Data Inicial *" hint="informe a data desejada" v-bind="attrs" @blur="dateDataInicial = parseDate(dateMovInicial)" v-on="on" ></v-text-field>
          </template>
            
          <v-date-picker v-model="dateDataInicial" no-title @input="menuEditDataInicial = false" locale="pt"></v-date-picker>
        </v-menu>

        <v-menu ref="menuEditDataFinal" v-model="menuEditDataFinal" :close-on-content-click="false" transition="scale-transition" offset-y max-width="290px" min-width="auto">
          <template v-slot:activator="{ on, attrs }">
            <v-text-field v-model="dateMovFinal" readonly label="Data Final *" hint="informe a data desejada" v-bind="attrs" @blur="dateDataFinal = parseDate(dateMovFinal)" v-on="on" ></v-text-field>
          </template>
            
          <v-date-picker v-model="dateDataFinal" no-title @input="menuEditDataFinal = false" locale="pt"></v-date-picker>
        </v-menu>

        <span class="titleS">Status:</span>
        <div class="modalStatusFooter">
          <span @click="changeStatusEdit(0, true)">
            <button id="statusEditPending">
              
            </button>
            Pendente
          </span>

          <span @click="changeStatusEdit(2, true)">
            <button id="statusEditInProgress">
              
            </button>
            Em andamento
          </span>
          
          <span @click="changeStatusEdit(1, true)">
            <button id="statusEditCompleted">
              
            </button>
            Concluída
          </span>
        </div>

        <span class="titleS">Categoria:</span>
        <div class="modalFooter">
          <div id="options">
            <span @click="changeCategoryEdit(0)">
              <button id="categoryEditUrgent">
                
              </button>
              Urgente
            </span>
            
            <span @click="changeCategoryEdit(1)">
              <button id="categoryEditImportant">
                
              </button>
              Importante
            </span>
            
            <span @click="changeCategoryEdit(2)">
              <button id="categoryEditOthers">
                
              </button>
              Outras
            </span>
          </div>

          <div id="crudButtons" style="margin-top: 2rem;">
            <button id="cancelEdit" @click="clearEditTaskData()">
              Cancelar
            </button>

            <button id="saveButton" @click="saveEdit()">
              Salvar
            </button>
          </div>
        </div>
      </div>
    </div>

    <div id="modalDelete" class="modalDelete">
      <div class="modal-content">
        <div id="icon">
          <font-awesome-icon icon="fa-regular fa-trash-can" />
        </div>

        <div>
          <h3>Tem certeza que deseja <span>excluir</span> essa tarefa?</h3>
          <span id="subTitle">Esta ação não pode ser desfeita</span>

          <button id="cancel" @click="cancelDelete()">
            Cancelar
          </button>
          
          <button id="confirm" @click="confirmDelete(idItemDelete)">
            Confirmar
          </button>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import '../assets/css/style.css';
import axios from 'axios';

window.onclick = function(event) {
  let modal = document.getElementById('myModal')
  let modalDelete = document.getElementById("modalDelete");
  let modalEdit = document.getElementById("modalEdit");
  
  if (event.target == modal) {
    document.getElementById("myModal").style.display = 'none';
    document.getElementById("categoryImportant").classList.remove('buttonActive');
    document.getElementById("categoryOthers").classList.remove('buttonActive');
    document.getElementById("categoryUrgent").classList.remove('buttonActive');
  }

  if (event.target == modalDelete) {
    document.getElementById("modalDelete").style.display = 'none';
  }
  
  if (event.target == modalEdit) {
    document.getElementById("categoryEditImportant").classList.remove('buttonActive');
    document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
    document.getElementById("categoryEditOthers").classList.remove('buttonActive');
    document.getElementById("modalEdit").style.display = 'none';
  }
}
export default {
  data(){
    return{
      title: '',
      description: '',
      category: '',
      codigoStatus: -1,
      descCategory: '',
      menuDataInicial: false,
      menuDataFinal: false,
      menuEditDataInicial: false,
      menuEditDataFinal: false,
      dateDataInicial: this.getFormattedDate(),
      dateDataFinal: this.getFormattedDate(),
      dateMovFinal: this.formatDate(this.getFormattedDate()),
      dateMovInicial: this.formatDate(this.getFormattedDate()),
      idItemDelete: '',
      codigoCategoria: -1,
      inputSearch: '',
      arrayFiltered:[
      ],
      modalEdit:{
        idTarefa: '',
        titulo: '',
        descricao: '',
        codigoStatus: -1,
        codigoCategoria: -1,
        category: 0
      },
      searchTable: '',
      headersDataTable: [
        { text: 'Título', align: 'start', value: 'titulo'},
        { text: 'Descrição', align: 'start', value: 'descricao'},
        { text: 'Categoria', value: 'codigoCategoria'},
        { text: 'Status', value: 'codigoStatus'},
        { text: 'Data Inicial', value: 'dataInicial' },
        { text: 'Data Final', align: 'start', value: 'dataFinal' },
        { text: 'Ações', value: 'action', sortable: false }
      ],
      itemsDataTable: [],
      itemsFilteredDataTable: [
        /*{
          //"idTarefa": 1,
          //"titulo": "Meu titulo",
          //"descricao": "Minha Descrição",
          //"codigoStatus": 1,
          //"codigoCategoria": 1,
          //"dataInicial": "2024-05-01T23:44:19.463Z",
          //"dataFinal": "2024-05-13T23:44:19.463Z"
        }*/
      ],
    }
  },
  methods: {
    filterDataDataTable (value, searchTable) {
      return  value != null &&
        searchTable != null &&
        typeof value === 'string' &&
        (value.toString().replace(".", ",").toLowerCase().indexOf(searchTable.toLowerCase()) !== -1 || 
        value.toString().replace(",", ".").toLowerCase().indexOf(searchTable.toLowerCase()) !== -1)
    },
    formatDate (date) {
      if (!date) return null;

      const [year, month, day] = date.split('-');
      return `${day}/${month}/${year}`;
    },
    parseDate (date) {
      if (!date) return null;

      const [day, month, year] = date.split('/');
      return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
    },
    async getTasks(){
      try {
        const response = await axios.get('http://localhost:5135/api/Task');
        this.itemsDataTable = await response.data;
        this.itemsFilteredDataTable = await response.data;
      } catch (error) {
        if (error.response) {
          console.log('Detalhes do erro:', error.response.data);
          console.log('Status do erro:', error.response.status);
          console.log('Cabeçalhos do erro:', error.response.headers);
        } else if (error.request) {
          console.log('O pedido foi feito mas não houve resposta', error.request);
        } else {
          console.log('Erro:', error.message);
        }

        alert('Falha ao buscar dados: verifique o console para mais informações.');
      }
    },
    login(){
      this.$emit("alterViewEmit", "Início");
    },
    inputFocus(){
      document.getElementById('searchTaskInput').focus();
    },
    editItem(item){
      this.modalEdit.idTarefa = item.idTarefa;
      this.modalEdit.titulo = item.titulo;
      this.modalEdit.descricao = item.descricao;
      this.modalEdit.codigoStatus = item.codigoStatus;
      this.dateDataInicial = this.toDate(item.dataInicial, 0);
      this.dateMovInicial = this.toDate(item.dataInicial, 1);
      this.dateDataFinal = this.toDate(item.dataFinal, 0);
      this.dateMovFinal = this.toDate(item.dataFinal, 1);

      if(item.codigoCategoria == 0){
        document.getElementById("categoryEditImportant").classList.remove('buttonActive');
        document.getElementById("categoryEditOthers").classList.remove('buttonActive');
        document.getElementById("categoryEditUrgent").classList.add('buttonActive');
        this.modalEdit.codigoCategoria = 0
        this.modalEdit.descCategory = 'Urgente'
      }
      else if(item.codigoCategoria == 1){
        document.getElementById("categoryEditImportant").classList.add('buttonActive');
        document.getElementById("categoryEditOthers").classList.remove('buttonActive');
        document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
        this.modalEdit.codigoCategoria = 1
        this.modalEdit.descCategory = 'Importante'
      } else if(item.codigoCategoria == 2){
        document.getElementById("categoryEditImportant").classList.remove('buttonActive');
        document.getElementById("categoryEditOthers").classList.add('buttonActive');
        document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
        this.modalEdit.codigoCategoria = 2
        this.modalEdit.descCategory = 'Outras'
      }

      document.getElementById("statusEditPending").classList.remove('buttonActive');
      document.getElementById("statusEditCompleted").classList.remove('buttonActive');
      document.getElementById("statusEditInProgress").classList.remove('buttonActive');
      if(item.codigoStatus == 0){
        document.getElementById("statusEditPending").classList.add('buttonActive');
      }
      else if(item.codigoStatus == 1){
        document.getElementById("statusEditCompleted").classList.add('buttonActive');
      }
      else if(item.codigoStatus == 2){
        document.getElementById("statusEditInProgress").classList.add('buttonActive');
      }
      
      document.getElementById("modalEdit").style.display = 'flex';
    }, 
    deleteItem(item){
      document.getElementById("modalDelete").style.display = 'flex';
      this.idItemDelete = item.idTarefa;
    },
    async saveEdit(){
      try {
        if(this.modalEdit.titulo.trim() == ""){
          alert("Título não pode ser vazio");
          return;
        }
        else if(this.modalEdit.titulo.length > 100){
          alert("Informe um título até 100 caracteres");
          return;
        }
        else if(this.modalEdit.descricao.length > 1000){
          alert("Informe uma descrição de no máximo 1000 caracteres");
          return;
        }
          
        await axios.put(`http://localhost:5135/api/Task/ID?ID=${this.modalEdit.idTarefa}`,{
          "idTarefa": this.modalEdit.idTarefa,
          "titulo": this.modalEdit.titulo,
          "descricao": this.modalEdit.descricao,
          "codigoStatus": this.modalEdit.codigoStatus,
          "codigoCategoria": this.modalEdit.codigoCategoria,
          "dataInicial": this.dateDataInicial,
          "dataFinal": this.dateDataFinal
        });
        alert("Alterado com sucesso");
        
        this.clearEditTaskData();
        this.getTasks();
        this.verify();
      } catch (error) {
        console.error("Erro ao excluir:", error);
        alert("Ocorreu um erro");
      }
    },
    toDate(isoDate, type){
      const date = new Date(isoDate);
      // Ajusta a data para o fuso horário local
      date.setMinutes(date.getMinutes() + date.getTimezoneOffset());
      
      // Obtém o ano, mês e dia
      const year = date.getFullYear();
      const month = (date.getMonth() + 1).toString().padStart(2, '0');
      const day = date.getDate().toString().padStart(2, '0');
      
      // Retorna a data no formato desejado
      if(type == 0) return `${year}-${month}-${day}`;
      if(type == 1) return `${day}/${month}/${year}`;
    },
    getFormattedDate() {
      const now = new Date();
      const year = now.getFullYear();
      const month = String(now.getMonth() + 1).padStart(2, '0'); // Mês começa em 0
      const day = String(now.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    },
    verify(){ // aqui temos que ver qual opção está ativa para filtrar apenas eles
      var allCategories = document.getElementById('allCategories').classList.contains("active");
      var onlyUrgent = document.getElementById('onlyUrgent').classList.contains("active");
      var onlyImportant = document.getElementById('onlyImportant').classList.contains("active");
      var onlyOthers = document.getElementById('onlyOthers').classList.contains("active");
      var onlyInProgress = document.getElementById('onlyInProgress').classList.contains("active");
      var onlyPending = document.getElementById('onlyInPending').classList.contains("active");
      var onlyFinished = document.getElementById('onlyFinished').classList.contains("active");

      if(allCategories) this.filterTasks('all');
      if(onlyUrgent) this.filterTasks('urgent');
      if(onlyImportant) this.filterTasks('important');
      if(onlyOthers) this.filterTasks('others');
      if(onlyInProgress) this.filterTasks('inProgress');
      if(onlyPending) this.filterTasks('inPending');
      if(onlyFinished) this.filterTasks('finished');
    },
    async confirmDelete(id){
      try {
        await axios.delete(`http://localhost:5135/api/Task/ID?ID=${id}`);
        alert("Excluído com sucesso");
        document.getElementById("modalDelete").style.display = 'none';
        this.getTasks();
      } catch (error) {
        console.error("Erro ao excluir:", error);
        alert("Ocorreu um erro");
      }
    },
    newTask(){
      this.titulo = '';
      this.descricao = '';
      this.codigoStatus = -1;
      this.codigoCategoria = -1;
      this.dataInicial = new Date();
      this.dataFinal = new Date();
      document.getElementById("myModal").style.display = 'flex';
    },
    clearNewTaskData(){
      this.title = '';
      this.description = '';
      this.codigoStatus = -1;
      this.codigoCategoria = -1;
      this.dateDataInicial = this.getFormattedDate();
      this.dateDataFinal = this.getFormattedDate();
      this.dateMovFinal = this.formatDate(this.getFormattedDate());
      this.dateMovInicial = this.formatDate(this.getFormattedDate());
      
      document.getElementById("myModal").style.display = 'none';
      document.getElementById("categoryImportant").classList.remove('buttonActive');
      document.getElementById("categoryOthers").classList.remove('buttonActive');
      document.getElementById("categoryUrgent").classList.remove('buttonActive');
      document.getElementById("statusPending").classList.remove('buttonActive');
      document.getElementById("statusInProgress").classList.remove('buttonActive');
      document.getElementById("statusCompleted").classList.remove('buttonActive');
    },
    clearEditTaskData(){
      this.modalEdit.idTarefa = 0;
      this.modalEdit.titulo = '';
      this.modalEdit.descricao = '';
      this.modalEdit.codigoStatus = -1;
      this.modalEdit.codigoCategoria = -1;
      this.dateDataInicial = this.getFormattedDate();
      this.dateDataFinal = this.getFormattedDate();
      this.dateMovFinal = this.formatDate(this.getFormattedDate());
      this.dateMovInicial = this.formatDate(this.getFormattedDate());
      document.getElementById("modalEdit").style.display = 'none';
      document.getElementById("categoryEditImportant").classList.remove('buttonActive');
      document.getElementById("categoryEditOthers").classList.remove('buttonActive');
      document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
    },
    cancelDelete(){
      document.getElementById("modalDelete").style.display = 'none';
    },
    activeClass(value){
      document.getElementById('allCategories').classList.remove("active")
      document.getElementById('onlyUrgent').classList.remove("active")
      document.getElementById('onlyImportant').classList.remove("active")
      document.getElementById('onlyOthers').classList.remove("active")
      document.getElementById('onlyInProgress').classList.remove("active")
      document.getElementById('onlyInPending').classList.remove("active")
      document.getElementById('onlyFinished').classList.remove("active")
      
      document.getElementById(value).classList.add("active")
    },
    async addTask(){
      if(this.title.trim() == ""){
        alert("Título não pode ser vazio");
        return;
      }
      else if(this.title.length > 100){
        alert("Informe um título até 100 caracteres");
        return;
      }
      else if(this.description.length > 1000){
        alert("Informe uma descrição de no máximo 1000 caracteres");
        return;
      }
      else if(this.codigoStatus == -1){
        alert("Informe um status");
        return;
      }
      else if(this.codigoCategoria == -1){
        alert("Informe uma categoria");
        return;
      }

      await axios.post('http://localhost:5135/api/Task', {
        titulo: this.title,
        descricao: this.description,
        codigoStatus: this.codigoStatus,
        codigoCategoria: this.codigoCategoria,
        dataInicial: this.dateDataInicial,
        dataFinal: this.dateDataFinal
      }).then(() => {
        alert("salvo com sucesso")
        this.getTasks();
      }).catch(() => {
        alert("ocorreu um erro")
      })

      this.clearNewTaskData();
    },
    changeCategory(value){
      if(value == 'urgent'){
        document.getElementById("categoryImportant").classList.remove('buttonActive');
        document.getElementById("categoryOthers").classList.remove('buttonActive');
        document.getElementById("categoryUrgent").classList.add('buttonActive');
        this.category = 'urgent'
        this.descCategory = 'Urgente'
        this.codigoCategoria = 0;
      }
      else if(value == 'important'){
        document.getElementById("categoryUrgent").classList.remove('buttonActive');
        document.getElementById("categoryOthers").classList.remove('buttonActive');
        document.getElementById("categoryImportant").classList.add('buttonActive');
        this.category = 'important'
        this.descCategory = 'Importante'
        this.codigoCategoria = 1;
      }
      else if(value == 'others'){
        document.getElementById("categoryUrgent").classList.remove('buttonActive');
        document.getElementById("categoryImportant").classList.remove('buttonActive');
        document.getElementById("categoryOthers").classList.add('buttonActive');
        this.category = 'others'
        this.descCategory = 'Outras'
        this.codigoCategoria = 2;
      }
    },
    changeCategoryEdit(value){
      if(value == 0){
        document.getElementById("categoryEditImportant").classList.remove('buttonActive');
        document.getElementById("categoryEditOthers").classList.remove('buttonActive');
        document.getElementById("categoryEditUrgent").classList.add('buttonActive');
        this.modalEdit.category = 0
        this.modalEdit.descCategory = 'Urgente'
        this.modalEdit.codigoCategoria = 0;
      }
      else if(value == 1){
        document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
        document.getElementById("categoryEditOthers").classList.remove('buttonActive');
        document.getElementById("categoryEditImportant").classList.add('buttonActive');
        this.modalEdit.category = 1
        this.modalEdit.descCategory = 'Importante'
        this.modalEdit.codigoCategoria = 1;
      }
      else if(value == 2){
        document.getElementById("categoryEditUrgent").classList.remove('buttonActive');
        document.getElementById("categoryEditImportant").classList.remove('buttonActive');
        document.getElementById("categoryEditOthers").classList.add('buttonActive');
        this.modalEdit.category = 2
        this.modalEdit.descCategory = 'Outras'
        this.modalEdit.codigoCategoria = 2;
      }
    },
    changeStatusEdit(value, modEdit){
      if(!modEdit){
        this.codigoStatus = value;
        document.getElementById("statusPending").classList.remove('buttonActive');
        document.getElementById("statusCompleted").classList.remove('buttonActive');
        document.getElementById("statusInProgress").classList.remove('buttonActive');

        if(value == 0){
          document.getElementById("statusPending").classList.add('buttonActive');
        }
        else if(value == 1){
          document.getElementById("statusCompleted").classList.add('buttonActive');
        }
        else if(value == 2){
          document.getElementById("statusInProgress").classList.add('buttonActive');
        }
      }
      else{
        this.modalEdit.codigoStatus = value;
        document.getElementById("statusEditPending").classList.remove('buttonActive');
        document.getElementById("statusEditCompleted").classList.remove('buttonActive');
        document.getElementById("statusEditInProgress").classList.remove('buttonActive');

        if(value == 0){
          document.getElementById("statusEditPending").classList.add('buttonActive');
        }
        else if(value == 1){
          document.getElementById("statusEditCompleted").classList.add('buttonActive');
        }
        else if(value == 2){
          document.getElementById("statusEditInProgress").classList.add('buttonActive');
        }
      }
    },
    filterTasks(value){
      if(value == 'all'){
        this.itemsFilteredDataTable = this.itemsDataTable;
      } 
      else if(value == 'urgent'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoCategoria == 0);
      }
      else if(value == 'important'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoCategoria == 1);
      }
      else if(value == 'others'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoCategoria == 2);
      }
      else if(value == 'inPending'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoStatus == 0);
      }
      else if(value == 'finished'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoStatus == 1);
      }
      else if(value == 'inProgress'){
        this.itemsFilteredDataTable = this.itemsDataTable.filter(element => element.codigoStatus == 2);
      }
    }
  },
  filters: {
    showMsgTasks(value){
      var msg = value == 0 ? 'nenhuma tarefa' : value == 1 ? value + ' tarefa' : value + ' tarefas'
      return msg
    },
    toDate(dateString){
      if (!dateString) return '';

      const date = new Date(dateString);
      const day = String(date.getUTCDate()).padStart(2, '0');
      const month = String(date.getUTCMonth() + 1).padStart(2, '0'); // Janeiro é 0
      const year = date.getUTCFullYear();

      return `${day}-${month}-${year}`;
    },
    totalUrgent(value){
      let data = value.filter(element => element.codigoCategoria == 0).length;
      return data;
    },
    totalImportant(value){
      let data = value.filter(element => element.codigoCategoria == 1).length;
      return data
    },
    totalOthers(value){
      let data = value.filter(element => element.codigoCategoria == 2).length;
      return data
    },
    toDescriptionDetail(value){
      return value == 0 ? "Pendente" : value == 1 ? "Concluída" : "Em Andamento"
    },
    toDescriptionCategoryDetail(value){
      return value == 0 ? "Urgente" : value == 1 ? "Importante" : "Outros"
    }
  },
  watch:{
    dateDataInicial () {
      this.dateMovInicial = this.formatDate(this.dateDataInicial);
    },
    dateDataFinal () {
      this.dateMovFinal = this.formatDate(this.dateDataFinal);
    },
  },
  computed: {
    computeddateMovInicial () {
      return this.formatDate(this.dateDataInicial)
    },
    computeddateMovFinal () {
      return this.formatDate(this.dateDataFinal)
    },
  },
  async created(){
    await this.getTasks();
  }
}
</script>