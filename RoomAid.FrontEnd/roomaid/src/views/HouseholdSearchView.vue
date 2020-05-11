<template>
  <div id = "SearchView">
    <v-container fluid>
      <SearchBar/>
      <SearchListing/>
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
  props: {
    households: []
  }
}
</script>
