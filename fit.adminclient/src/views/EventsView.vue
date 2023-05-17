<script setup>
import axios from "axios";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import ColumnGroup from 'primevue/columngroup';
import Row from 'primevue/row'; 
</script>



<template>
  <DataTable v-model:expandedRows="expandedRows" :value="events" dataKey="name" @rowExpand="onRowExpand"
    @rowCollapse="onRowCollapse" tableStyle="min-width: 20rem">
    <Column expander style="width: 2rem" />
    <Column field="name" header="Name"></Column>
    <Column field="date" header="date"></Column>
    <Column :exportable="false" style="min-width:8rem">
        <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="addPackage()" />
            <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteProduct(slotProps.data)" />
        </template>
    </Column>
    <template #expansion="slotProps">
      <div class="p-3">
        <DataTable :value="slotProps.data.packages">
          <Column field="name" header="Name" sortable>
          </Column>
        </DataTable>
      </div>
    </template>
  </DataTable>
</template>

<script>

 
export default {
  data() {
    return {
      events: [],
    };
  },
  methods: {
    async getAllEvents() {
      try {
        this.events = (await axios.get(`/event`)).data;
      } catch (e) {
        alert("Fehler beim Laden der Events.");
      }
    },
  },
  async mounted() {
    await this.getAllEvents();
  }
};
</script>