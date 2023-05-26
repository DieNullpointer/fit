<script setup>
import { ref } from "vue";
import axios from "axios";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import ColumnGroup from 'primevue/columngroup';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Row from 'primevue/row';
import { FilterMatchMode } from 'primevue/api';

import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

</script>

<template>
    <div class="packagesView">
        <DataTable ref="dt" :value="packages" dataKey="guid" :filters="filters">
            <template #header>
                <div class="flex flex-wrap gap-2  justify-content-between">
                    <h4 class="m-0">Manage Packages</h4>
                    <span class="p-input-icon-left">
                        <i class="pi pi-search" />
                        <InputText v-model="filters['global'].value" placeholder="Search..." />
                    </span>
                    <Button type="button" @click="newPackage()" label="New Package" />
                </div>
            </template>
            <Column field="name" header="Name" sortable style="min-width:12rem"></Column>
            <Column field="price" header="Price" sortable style="min-width:16rem"></Column>
            <Column :exportable="false" style="min-width:8rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editPackage(slotProps.data)" />
                </template>
            </Column>
        </DataTable>
    </div>


    <Dialog v-model:visible="dialog" :style="{ width: '450px' }" header="Package Details" :modal="true"
        class="p-fluid">
        <div class="field">
            <label for="name">Name</label>
            <InputText id="name" v-model.trim="package.name" required="true" autofocus
                :class="{ 'p-invalid': submitted && !package.name }" />
            <small class="p-error" v-if="submitted && !package.name">Name is required.</small>
        </div>

        <div class="p-fluid">
            <div class="field">
                <label for="price">Price</label>
                <InputText id="name" v-model.trim="package.price" required="true" autofocus
                :class="{ 'p-invalid': submitted && !package.price }" />
            <small class="p-error" v-if="submitted && !package.price">Price is required.</small>
            </div>
        </div>
        <template #footer>
            <Button label="Cancel" icon="pi pi-times" text @click="hideDialog()" />
            <Button label="Save" icon="pi pi-check" text @click="savePackage()" />
        </template>
    </Dialog>
</template>

<script>
export default {
    data() {
        return {
            packages: [],
            package: {
                guid: '',
                name: '',
                price: ''
            },
            filters: {},
            dialog: false,
            submitted: false,
        };
    },
    created() {
        this.initFilters();
    },
    async mounted() {
        await this.getAllPackages();
    },
    methods: {
        async getAllPackages() {
            try {
                this.packages = (await axios.get(`/package`)).data;
            } catch (e) {
                alert("Fehler beim Laden der Package.");
            }
        },
        async addPackage() {
            this.submitted = true;
            try {
                await axios.post('package/add', this.package)
                console.log(this.package)
                this.hideDialog();
                await this.getAllPackages();
            } catch (e) {
                console.log("Fehler bie Add Package");
            }
        },
        async changePackage() {
            console.log(this.package)
            try {
                await axios.put('package/change', this.package)
                this.hideDialog();
                await this.getAllPackages();
            } catch (e) {
                console.log(e.response)
            }
        },
        newPackage() {
            this.package.name = '';
            this.package.price = '';
            this.submitted = false;
            this.dialog = true;
        },
        editPackage(item) {
            this.package.guid = item.guid;
            this.package.name = item.name;
            this.package.price = item.price +'';
            this.packageDialog = true;
        },
        hideDialog () {
            this.dialog = false;
            this.submitted = false;
            this.package.guid = '';
            this.package.name = '';
            this.package.price = '';
            this.submitted = false;
            this.packageDialog = true;
        },
        hideDialog () {
            this.dialog = false;
            this.submitted = false;
            this.package.guid = '';
            this.package.name = '';
            this.package.price = '';
        },
        savePackage() {
            this.submitted = true;
			if (this.package.name.trim() && this.package.price.trim()) {
                if (this.package.guid) {
                    this.changePackage();
                }
                else {
                    this.addPackage()
                }
                this.hideDialog()
            }
        },
        findIndexById(id) {
            let index = -1;
            for (let i = 0; i < this.packages.length; i++) {
                if (this.packages[i].id === id) {
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
