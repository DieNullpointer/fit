<script setup>
import { ref } from "vue";
import axios from "axios";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import { FilterMatchMode } from 'primevue/api';
import Calendar from 'primevue/calendar';

import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

</script>

<template>
  <div class="companiesView">
    <DataTable ref="dt" v-model:expandedRows="expandedRows" :value="events" dataKey="guid" :filters="filters">
      <template #header>
        <div class="flex flex-wrap gap-2  justify-content-between">
          <h4 class="m-0">Manage Events</h4>
          <span class="p-input-icon-left">
            <i class="pi pi-search" />
            <InputText v-model="filters['global'].value" placeholder="Search..." />
          </span>
          <Button type="button" @click="newEvent()" label="New Event" />
        </div>
      </template>
      <Column expander style="width: 5rem" />
      <Column field="name" header="Name" sortable style="min-width:12rem"></Column>
      <Column field="date" header="Date" sortable style="min-width:12rem"></Column>
      <Column :exportable="false" style="min-width:8rem">
        <template #body="slotProps">
          <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editEvent(slotProps.data)" />
        </template>
      </Column>
      <template #expansion="slotProps">
        <div class="p-3">
          <DataTable :value="slotProps.data.packages">
            <Column field="name" header="name" sortable></Column>
          </DataTable>
        </div>
      </template>
    </DataTable>
  </div>


  <Dialog v-model:visible="dialog" :style="{ width: '450px' }" header="Event Details" :modal="true" class="p-fluid">
    <div class="field">
      <label for="name">Name</label>
      <InputText id="name" v-model.trim="event.name" required="true" autofocus
        :class="{ 'p-invalid': submitted && !event.name }" />
      <small class="p-error" v-if="submitted && !event.name">Name is required.</small>
    </div>

    <div class="p-fluid">
      <div class="field">
        <label for="date">Date</label>
        <Calendar id="name" v-model.trim="event.date" required="true" autofocus
          :class="{ 'p-invalid': submitted && !event.date }" />
        <small class="p-error" v-if="submitted && !event.date">Price is required.</small>
      </div>
    </div>
    <template #footer>
      <Button label="Cancel" icon="pi pi-times" text @click="hideDialog()" />
      <Button label="Save" icon="pi pi-check" text @click="changeEvent()" />
    </template>
  </Dialog>
</template>

<script>
export default {
  data() {
    return {
      events: [],
      event: {
        guid: '',
        name: '',
        date: '',
        packages: []
      },
      filters: {},
      dialog: false,
      submitted: false,

      expandedRows: ref([])
    }
  },
  created() {
    this.initFilters();
  },
  async mounted() {
    await this.getAllEvents();
  },
  methods: {
    async getAllEvents() {
      try {
        this.events = (await axios.get(`/event`)).data;
      } catch (e) {
        alert("Fehler beim Laden der Companies.");
      }
    },
    async addEvent() {
      this.submitted = true;
      try {
        await axios.post('event/add', this.event)
        console.log(this.event)
        this.hideDialog();
        await this.getAllEvents();
      } catch (e) {
        console.log("Fehler bei Add Evente");
      }
    },
    async changeEvent() {
      this.submitted = true;
      if (this.event.name.trim()) {
        try {
          await axios.put('event/change', this.event)
          this.hideDialog();
          await this.getAllEvents();
        } catch (e) {
          console.log(e.response)
        }
      }

    },
    newEvent() {
      this.event.name = '';
      this.event.date = '';
      this.event.packages = [];
      this.submitted = false;
      this.dialog = true;
    },
    editEvent(item) {
      this.event.guid = item.guid;
      this.event.name = item.name;
      this.event.date = item.date + '';
      this.event.packages = item.packages;
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.submitted = false;
      this.event.guid = '';
      this.event.name = '';
      this.event.date = '';
      this.event.packages = [];
    },
    saveEvent() {
      this.submitted = true;
      if (this.event.name.trim() && this.event.date.trim()) {
        if (this.event.guid) {
          this.changeEvent();
        }
        else {
          this.addEvent()
        }
        this.hideDialog()
      }
    },
    findIndexById(id) {
      let index = -1;
      for (let i = 0; i < this.events.length; i++) {
        if (this.events[i].id === id) {
          index = i;
          break;
        }
      }
      return index;
    },
    createId() {
      let id = '';
      var chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
      for (var i = 0; i < 5; i++) {
        id += chars.charAt(Math.floor(Math.random() * chars.length));
      }
      return id;
    },
    initFilters() {
      this.filters = {
        'global': { value: null, matchMode: FilterMatchMode.CONTAINS },
      }
    },
  }
};
</script>
