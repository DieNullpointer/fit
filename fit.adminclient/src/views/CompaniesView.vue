<script setup>
import { ref } from "vue";
import axios from "axios";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import { FilterMatchMode } from 'primevue/api';

import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

</script>

<template>
    <div class="companiesView">
        <DataTable ref="dt" :value="companies" dataKey="guid" :filters="filters">
            <template #header>
                <div class="flex flex-wrap gap-2  justify-content-between">
                    <h4 class="m-0">Manage Packages</h4>
                    <span class="p-input-icon-left">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Search..." />
                    </span>
                </div>
            </template>
            <Column field="name" header="Name" sortable style="min-width:12rem"></Column>
            <Column field="eventName" header="Event" sortable style="min-width:12rem"></Column>
            <Column field="packageName" header="Package" sortable style="min-width:16rem"></Column>
            <Column :exportable="false" style="min-width:8rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editCompany(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteCompany(slotProps.data)" />
                </template>
            </Column>
        </DataTable>
    </div>


    <Dialog v-model:visible="changeDialog" :style="{ width: '450px' }" header="Package Details" :modal="true" class="p-fluid">
        <div class="field">
            <label for="name">Name</label>
            <InputText id="name" v-model.trim="company.name" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.name }" />
            <small class="p-error" v-if="submitted && !company.name">Name is required.</small>
        </div>
        <template #footer>
            <Button label="Cancel" icon="pi pi-times" text @click="hideDialog()" />
            <Button label="Save" icon="pi pi-check" text @click="savePackage()" />
        </template>
    </Dialog>

    <Dialog v-model:visible="deleteDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
            <div class="confirmation-content">
                <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
                <span v-if="company">Are you sure you want to delete <b>{{company.name}}</b>?</span>
            </div>
            <template #footer>
                <Button label="No" icon="pi pi-times" text @click="hideDialog()"/>
                <Button label="Yes" icon="pi pi-check" text @click="deleteCompany()" />
            </template>
        </Dialog>
</template>

<script>
export default {
    data() {
        return {
            companies: [],
            company: {
                guid: '',
                name: '',
                address: '',
                country: '',
                plz: '',
                place: '',
                billAddress: '',
                eventName: '',
                packageName: '',
                patners: []
            },
            filters: {},
            changeDialog: false,
            submitted: false,
            deleteDialog: false
        };
    },
    created() {
        this.initFilters();
    },
    async mounted() {
        await this.getAllCompanies();
    },
    methods: {
        async getAllCompanies() {
            try {
                this.companies = (await axios.get(`/company`)).data;
            } catch (e) {
                alert("Fehler beim Laden der Companies.");
            }
        },
        async changeCompany() {
            console.log(this.company)
            try {
                await axios.put('company/change', this.company)
                this.hideDialog();
                await this.getAllCompanies();
            } catch (e) {
                console.log(e.response)
            }
        },
        async deleteCompany() {
            console.log(this.company)
            try {
                await axios.delete(`company/delete${this.company.guid}`)
                this.hideDialog();
                await this.getAllCompanies();
            } catch (e) {
                console.log(e.response)
            }
        },
        editCompany(item) {
            this.company.guid = item.guid;
            this.company.name = item.name;
            this.changeDialog = true;
        },
        confirmDeleteCompany(item) {
            this.company.guid = item.guid;
            this.deleteDialog = true;
        },
        hideDialog() {
            this.changeDialog = false;
            this.submitted = false;
            this.deleteDialog = false
            this.company.guid = '';
            this.company.name = '';
        },
        saveCompany() {
            this.submitted = true;
            if (this.company.name.trim()) {
                this.changePackage();
                this.hideDialog()
            }
        },
        findIndexById(id) {
            let index = -1;
            for (let i = 0; i < this.companies.length; i++) {
                if (this.companies[i].id === id) {
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
