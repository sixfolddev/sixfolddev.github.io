<template>
    <div id = "HouseholdSearchView">
        <v-container fluid>
            <v-toolbar
            dark
            round
            class = "mx-10"
            >
                <v-toolbar-title>Search For Households: </v-toolbar-title>
                <v-autocomplete
                    v-model="searchRequest.cityName"
                    :loading="loading"
                    :items="items"
                    :search-input.sync="search"
                    cache-items
                    class="mx-6"
                    flat
                    hide-no-data
                    hide-details
                    no-data-text="This city does not exist in California!"
                    label="Enter a city"
                    solo-inverted
                    auto-select-first
                    append-icon=""
                ></v-autocomplete>
                    <v-btn icon outlined @click="HouseholdSearch">
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
                    v-model="searchRequest.householdType"
                ></v-select>
                </v-col>
                <v-col cols = "3" class = "mt-8 pt-0">
                <v-text-field
                    single-line
                    label="Minimum"
                    prepend-icon="mdi-currency-usd"
                    clearable
                    v-model="searchRequest.minPrice"
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
                    v-model="searchRequest.maxPrice"
                >
                </v-text-field>
                 </v-col>
            </v-toolbar>
            <v-alert
              :value="errorMessage"
              id="errorMessage"
              type="error"
              transition="scale-transition"
              class = "mx-10"
              v-if="!(errorMessage.length === 0)">
                {{errorMessage}}
            </v-alert>
        <!-- <p id = "results">Households: {{this.households}}</p>
        <p id = "count">Total Returned Values: {{this.households.length}}</p> -->
        </v-container>
    </div>
</template>
<script>

// import * as config from './config.prod.json'

export default {
  name: 'HouseholdSearchView',
  data () {
    return {
      loading: false,
      items: [],
      search: null,
      select: null,
      householdTypes: ['All', 'Apartment', 'Townhouse', 'House'],
      cities: [],
      searchRequest: {
        cityName: '',
        householdType: '',
        minPrice: null,
        maxPrice: null,
        page: 1
      },
      errorMessage: [],
      count: null,
      totalResultCount: null,
      households: []
    }
  },
  watch: {
    search (val) {
      val && val !== this.select && this.querySelections(val)
      // .then((r) => this.loading = r)
    }
  },
  // Run a query to get the list of cities required for autocomplete as the page loads
  beforeMount () {
    this.Autocomplete()
  },
  methods: {
    // Function used to get a list of search results from the server
    // Also validates search inputs
    HouseholdSearch: function () {
      this.errorMessage = []
      if (!(this.cities.includes(this.searchRequest.cityName)) || this.searchRequest.cityName.length === 0) {
        this.errorMessage.push('Please select a city from the list!')
      }
      if (this.searchRequest.minPrice === null) {
        this.searchRequest.minPrice = 0
      }
      if (this.searchRequest.maxPrice === null) {
        this.searchRequest.maxPrice = 10000
      }
      if (isNaN(this.searchRequest.maxPrice) || isNaN(this.searchRequest.minPrice)) {
        this.errorMessage.push('The price filters must be a number!')
      }
      if (this.searchRequest.maxPrice < 0 || this.searchRequest.maxPrice > 10000) {
        this.errorMessage.push('The maximum price filter must be between 0 and 10000')
      }
      if (this.searchRequest.minPrice < 0 || this.searchRequest.minPrice > 10000) {
        this.errorMessage.push('The minimum price filter must be between 0 and 10000')
      }
      if (this.searchRequest.householdType === '' || this.searchRequest.householdType === 'All') {
        this.searchRequest.householdType = 'none'
      }
      if (this.errorMessage.length === 0) {
        this.GetTotalResultCount()
        this.GetSearchResults()
        // HACK: WHY IS THIS ONLY WORKING ON DOUBLE CLICK?!??
        this.$emit('input', this.households)
      }
    },
    // Used for filtering out option when typing in the autocomplete
    querySelections (v) {
      this.loading = true
      setTimeout(() => {
        this.items = this.cities.filter(e => {
          return (e || '').toLowerCase().indexOf((v || '').toLowerCase()) > -1
        })
        this.loading = false
      }, 500)

      // return new Promise((resovle, reject) => {

      //     this.items = this.cities.filter(e => {
      //     return resolve((e || '').toLowerCase().indexOf((v || '').toLowerCase()) > -1)
      //     }

      //     return reject(false)

      // })
    },
    // Function used to get a list of cities from server
    Autocomplete () {
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

    // Get all results for a query using the searchRequest data
    GetSearchResults () {
      const params = {
        cityName: this.searchRequest.cityName,
        page: this.searchRequest.page,
        minPrice: this.searchRequest.minPrice,
        maxPrice: this.searchRequest.maxPrice,
        householdType: this.searchRequest.householdType
      }
      const query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&')
      const uri = `${this.$hostname}/api/search/householdsearch?` + query
      const req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => response.json())
        .then(data => {
          this.households = (data)
        })
    },
    // Function used for getting the total number of results for pagination
    GetTotalResultCount () {
      const params = {
        cityName: this.searchRequest.cityName,
        minPrice: this.searchRequest.minPrice,
        maxPrice: this.searchRequest.maxPrice,
        householdType: this.searchRequest.householdType
      }
      const query = Object.keys(params)
        .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k]))
        .join('&')
      const uri = `${this.$hostname}/api/search/count?` + query
      const req = new Request(uri, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        mode: 'cors'
      })
      fetch(req)
        .then(response => response.json())
        .then(data => {
          this.totalResultCount = (data)
        })
    }
  }
}
</script>
