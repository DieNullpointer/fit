<script setup>
import { ref } from "vue";
import axios from "axios";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import { FilterMatchMode } from 'primevue/api';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import Dropdown from 'primevue/dropdown';

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
            <Column field="package.name" header="Package" sortable style="min-width:16rem"></Column>
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
                    <Button label="Download" @click="download(slotProps.data.guid)" />
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
                        <Column :exportable="false" style="min-width:8rem">
                            <template #body="slotProps">
                                <Button icon="pi pi-pencil" outlined rounded class="mr-2"
                                    @click="editPatner(slotProps.data)" />
                            </template>
                        </Column>
                    </DataTable>
                </div>
            </template>
        </DataTable>
    </div>

    <Dialog v-model:visible="changeCompanyDialog" :style="{ width: '450px' }" header="Package Details" :modal="true"
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

    <Dialog v-model:visible="changePatnerDialog" :style="{ width: '450px' }" header="Package Details" :modal="true"
        class="p-fluid">
        <div class="field">
            <label for="title">Title</label>
            <InputText id="title" v-model.trim="patner.title" required="true" autofocus />
        </div>

        <div class="field">
            <label for="firstname">Firstname</label>
            <InputText id="firstname" v-model.trim="patner.firstname" required="true" autofocus
                :class="{ 'p-invalid': submitted && !patner.firstname }" />
            <small class="p-error" v-if="submitted && !patner.firstname">Firstname is required.</small>
        </div>

        <div class="field">
            <label for="lastname">Lastname</label>
            <InputText id="lastname" v-model.trim="patner.lastname" required="true" autofocus
                :class="{ 'p-invalid': submitted && !patner.lastname }" />
            <small class="p-error" v-if="submitted && !patner.lastname">Lastname is required.</small>
        </div>

        <div class="field">
            <label for="email">E-Mail</label>
            <InputText id="email" v-model.trim="patner.email" required="true" autofocus
                :class="{ 'p-invalid': submitted && !patner.email }" />
            <small class="p-error" v-if="submitted && !patner.email">E-Mail is required.</small>
        </div>

        <div class="field">
            <label for="telNr">telNr</label>
            <InputText id="telNr" v-model.trim="patner.telNr" required="true" autofocus
                :class="{ 'p-invalid': submitted && !patner.telNr }" />
            <small class="p-error" v-if="submitted && !patner.telNr">telNr is required.</small>
        </div>

        <div class="field">
            <label for="mobilNr">mobilNr</label>
            <InputText id="mobilNr" v-model.trim="patner.mobilNr" autofocus />
        </div>

        <div class="field">
            <label for="function">function</label>
            <InputText id="function" v-model.trim="patner.function" required="true" autofocus
                :class="{ 'p-invalid': submitted && !patner.function }" />
            <small class="p-error" v-if="submitted && !patner.function">function is required.</small>
        </div>

        <div class="field">
            <label for="mainPartner">mainPartner</label>
            <Dropdown v-model="patner.mainPartner" :options="options" optionLabel="name" class="w-full md:w-14rem" />
            <small class="p-error" v-if="submitted && !patner.mainPartner">mainPartner is required.</small>
        </div>

        <template #footer>
            <Button label="Cancel" icon="pi pi-times" text @click="hideDialog()" />
            <Button label="Save" icon="pi pi-check" text @click="changePatner()" />
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
                package: '',
                partners: []
            },
            patner: {
                guid: '',
                title: '',
                firstname: '',
                lastname: '',
                email: '',
                telNr: '',
                mobilNr: '',
                function: '',
                mainPartner: ''
            },
            filters: {},
            changeCompanyDialog: false,
            changePatnerDialog: false,
            submitted: false,
            deleteDialog: false,
            expandedRows: ref([]),
            options: ref([
                { name: "true" },
                { name: "false" }
            ])
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
            if (this.company.name.trim() &&
                this.company.address.trim() &&
                this.company.country.trim() &&
                this.company.plz.trim() &&
                this.company.place.trim() &&
                this.company.billAddress.trim()) {
                try {
                    await axios.put('company/change', this.company)
                    this.hideDialog();
                    await this.getAllCompanies();
                } catch (e) {
                    console.log(e.response)
                }
            }
        },
        async changePatner() {
            this.submitted = true;
            if (this.patner.email.trim() && this.patner.firstname.trim() && this.patner.lastname.trim() && this.patner.function.trim() && this.patner.telNr.trim()) {
                try {
                    await axios.put('patner/change', this.patner)
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
        async download(company) {
            window.location.href = `${axios.defaults.baseURL}/company/getFiles/${company}?fileName=all`
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
            this.company.packageGuid = item.package.guid;
            this.company.partners = item.partners;
            this.changeCompanyDialog = true;
        },
        editPatner(item) {
            this.patner.guid = item.guid;
            this.patner.title = item.title
            this.patner.firstname = item.firstname
            this.patner.lastname = item.lastname
            this.patner.email = item.email
            this.patner.telNr = item.telNr
            this.patner.mobilNr = item.mobilNr
            this.patner.function = item.function
            this.patner.mainPartner = item.mainPartner
            console.log(this.patner)
            this.changePatnerDialog = true;
        },
        confirmDeleteCompany(item) {
            this.company.guid = item.guid;
            this.deleteDialog = true;
        },
        hideDialog() {
            this.changeCompanyDialog = false;
            this.changePatnerDialog = false;
            this.submitted = false;
            this.deleteDialog = false;
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