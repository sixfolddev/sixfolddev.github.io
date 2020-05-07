<template>
    <div id = "HouseholdSearchView">
      <p>{{}}</p>
        <v-container fluid>
            <v-toolbar
            dark
            class = "mx-10"
            >
                <v-toolbar-title>Search For Households: </v-toolbar-title>
                <v-autocomplete
                    v-model="select"
                    :loading="loading"
                    :items="items"
                    :search-input.sync="search"
                    cache-items
                    class="mx-6"
                    flat
                    hide-no-data
                    hide-details
                    label="Enter a city"
                    solo-inverted
                    auto-select-first="true"
                ></v-autocomplete>
                    <v-btn icon outlined>
                        <v-icon>mdi-magnify</v-icon>
                    </v-btn>
            </v-toolbar>
            <v-divider class = "mx-10"></v-divider>
            <v-toolbar
            dark
            class = "mx-10"
            dense
            >
                <v-toolbar-title class = "mx-3">Filters:</v-toolbar-title>
                <v-col cols = "4">
                <v-select
                    class = "mx-1"
                    :items="householdTypes"
                    menu-props="auto"
                    label="Type"
                    hide-details
                    single-line
                    clearable
                ></v-select>
                </v-col>
                <v-col cols = "3" class = "mt-8 pt-0">
                <v-text-field
                    single-line
                    label="Minimum"
                    prepend-icon="mdi-currency-usd"
                    clearable
                >
                </v-text-field>
                </v-col>
                 <v-col cols = "3" class = "mt-8 pt-0">
                <v-text-field
                    single-line
                    class = "mx-.5"
                    label="Maximum"
                    prepend-icon="mdi-currency-usd"
                    clearable
                >
                </v-text-field>
                 </v-col>
            </v-toolbar>
        </v-container>
    </div>
</template>
<script>
export default {
  name: 'HouseholdSearchView',
  data () {
    return {
      loading: false,
      items: [],
      search: null,
      select: null,
      householdTypes: ['Apartment', 'Townhouse', 'House'],
      cities: [],
      cityName: '',
      householdtype: '',
      minPrice: '',
      maxPrice: '',
      page: null,
      errorMessage: null,
      data: {
        Households: []
      },
      count: null,
      testValue: ''
    }
  },
  watch: {
    search (val) {
      val && val !== this.select && this.querySelections(val)
    }
  },
  beforeMount () {
    this.autocomplete()
  },
  methods: {
    querySelections (v) {
      /// Ajax Call Goes Here
      this.loading = true
      setTimeout(() => {
        this.items = this.cities.filter(e => {
          return (e || '').toLowerCase().indexOf((v || '').toLowerCase()) > -1
        })
        this.loading = false
      }, 500)
    },
    autocomplete () {
      const uri = `${this.$hostname}/api/search/autocomplete`
      const req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => response.json())
        .then(data => {
          this.cities = data
        })
    },
    test () {
      const uri = `${this.$hostname}/api/search/test`
      const req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => response.json())
        .then(data => {
          this.testValue = data.toString()
        })
    }
  }
}
</script>
