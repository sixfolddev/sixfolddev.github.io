<template>
  <v-card
    class="mx-10"
  >
  <span>Households: {{this.households}}</span>
    <v-app-bar
      dark
    >

      <v-toolbar-title>Search Results For: </v-toolbar-title>

      <v-spacer></v-spacer>
    </v-app-bar>

    <v-container>
      <v-row dense>
        <v-col
          v-for="(item, i) in households"
          :key="i"
          cols="12"
        >
          <v-card
            dark
          >
            <div class="d-flex flex-no-wrap justify-space-between">
              <div>
                <v-card-title
                  class="headline"
                  v-text="households.householdType"
                ></v-card-title>

                <v-card-subtitle v-text="households.listingDescription"></v-card-subtitle>
              </div>

              <v-avatar
                class="ma-3"
                size="125"
                tile
              >
                <v-img :src="item.src"></v-img>
              </v-avatar>
            </div>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
export default {
  data: () => ({
    households: []
  }),
  beforemount () {
    this.households = this.$store.getters.searchResults
  }
}
</script>
