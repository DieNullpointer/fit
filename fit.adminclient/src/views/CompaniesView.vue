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
        <DataTable ref="dt" v-model:expandedRows="expandedRows" :value="companies" dataKey="guid" :filters="filters">
            <template #header>
                <div class="flex flex-wrap gap-2  justify-content-between">
                    <h4 class="m-0">Manage Packages</h4>
                    <span class="p-input-icon-left">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Search..." />
                    </span>
                </div>
            </template>
            <Column expander style="width: 5rem" />
            <Column field="name" header="Name" sortable style="min-width:12rem"></Column>
            <Column field="event.name" header="Event" sortable style="min-width:12rem"></Column>
            <Column field="packageName.name" header="Package" sortable style="min-width:16rem"></Column>
            <Column field="address" header="address" sortable></Column>
            <Column field="country" header="country" sortable></Column>
            <Column field="plz" header="plz" sortable></Column>
            <Column field="place" header="place" sortable></Column>
            <Column field="billAddress" header="billAddress" sortable></Column>
            <Column :exportable="false" style="min-width:8rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editCompany(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger"
                        @click="confirmDeleteCompany(slotProps.data)" />
                </template>
            </Column>
            <template #expansion="slotProps">
                <div class="p-3">
                    <DataTable :value="slotProps.data.partners">
                        <Column field="title" header="title" sortable></Column>
                        <Column field="firstname" header="firstname" sortable></Column>
                        <Column field="lastname" header="lastname" sortable></Column>
                        <Column field="email" header="email" sortable></Column>
                        <Column field="telNr" header="telNr" sortable></Column>
                        <Column field="mobilNr" header="mobilNr" sortable></Column>
                        <Column field="function" header="function" sortable></Column>
                        <Column field="mainPartner" header="mainPartner" sortable></Column>
                    </DataTable>
                </div>
            </template>
        </DataTable>
    </div>


    <Dialog v-model:visible="changeDialog" :style="{ width: '450px' }" header="Package Details" :modal="true"
        class="p-fluid">
        <div class="field">
            <label for="name">Name</label>
            <InputText id="name" v-model.trim="company.name" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.name }" />
            <small class="p-error" v-if="submitted && !company.name">Name is required.</small>
        </div>

        <div class="field">
            <label for="address">Addresse</label>
            <InputText id="address" v-model.trim="company.address" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.address }" />
            <small class="p-error" v-if="submitted && !company.address">Address is required.</small>
        </div>

        <div class="field">
            <label for="country">Land</label>
            <InputText id="country" v-model.trim="company.country" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.country }" />
            <small class="p-error" v-if="submitted && !company.country">Country is required.</small>
        </div>

        <div class="field">
            <label for="plz">Plz</label>
            <InputText id="plz" v-model.trim="company.plz" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.plz }" />
            <small class="p-error" v-if="submitted && !company.plz">Plz is required.</small>
        </div>

        <div class="field">
            <label for="place">Place</label>
            <InputText id="place" v-model.trim="company.place" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.place }" />
            <small class="p-error" v-if="submitted && !company.place">Place is required.</small>
        </div>

        <div class="field">
            <label for="billAddress">billAddress</label>
            <InputText id="billAddress" v-model.trim="company.billAddress" required="true" autofocus
                :class="{ 'p-invalid': submitted && !company.billAddress }" />
            <small class="p-error" v-if="submitted && !company.billAddress">billAddress is required.</small>
        </div>

        <template #footer>
            <Button label="Cancel" icon="pi pi-times" text @click="hideDialog()" />
            <Button label="Save" icon="pi pi-check" text @click="changeCompany()" />
        </template>
    </Dialog>

    <Dialog v-model:visible="deleteDialog" :style="{ width: '450px' }" header="Confirm" :modal="true">
        <div class="confirmation-content">
            <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
            <span v-if="company">Are you sure you want to delete <b>{{ company.name }}</b>?</span>
        </div>
        <template #footer>
            <Button label="No" icon="pi pi-times" text @click="hideDialog()" />
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
                event: '',
                packageName: '',
                partners: []
            },
            filters: {},
            changeDialog: false,
            submitted: false,
            deleteDialog: false,

            expandedRows: ref([])
        }
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
            this.submitted = true;
            if (this.company.name.trim()) {
                try {
                    await axios.put('company/change', this.company)
                    this.hideDialog();
                    await this.getAllCompanies();
                } catch (e) {
                    console.log(e.response)
                }
            }

        },
        async deleteCompany() {

            try {
                await axios.delete(`company/delete/${this.company.guid}`)
                this.hideDialog();
                await this.getAllCompanies();
            } catch (e) {
                console.log(e.response)
            }
        },
        editCompany(item) {
            this.company.guid = item.guid;
            this.company.name = item.name;
            this.company.address = item.address;
            this.company.country = item.country;
            this.company.plz = item.plz;
            this.company.place = item.place;
            this.company.billAddress = item.billAddress;
            this.company.eventGuid = item.event.guid;
            this.company.packageGuid = item.packageName.guid;
            this.company.partners = item.partners;
            console.log(this.company.event)
            this.changeDialog = true;
        },
        confirmDeleteCompany(item) {
            this.company.guid = item.guid;
            console.log(this.company.guid)
            this.deleteDialog = true;
        },
        hideDialog() {
            this.changeDialog = false;
            this.submitted = false;
            this.deleteDialog = false
            this.company.guid = '';
            this.company.name = '';
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
