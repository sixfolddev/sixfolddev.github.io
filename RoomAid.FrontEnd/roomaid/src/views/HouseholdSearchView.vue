<template>
  <div id = "SearchView">
    <v-container fluid>
      <SearchBar v-model = "households"/>
      <SearchListing :households = "households" />
    </v-container>
  </div>
</template>
<script>
import SearchBar from '@/components/SearchBar.vue'
import SearchListing from '@/components/SearchListing.vue'
export default {
  name: 'SearchView',
  components: {
    SearchBar,
    SearchListing
  },
  data () {
    return { households: [] }
  }
}
</script>
