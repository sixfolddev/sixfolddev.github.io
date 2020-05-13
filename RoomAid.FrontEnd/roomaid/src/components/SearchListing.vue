<template>
<div class = "listing">
  <v-card
    class="mx-10"
  >
    <v-app-bar
      dark
    >

      <v-toolbar-title>Search Results: </v-toolbar-title>
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
                  v-text=" 'Hostname Placeholder: ' + item.householdType"
                ></v-card-title>
                <v-card-subtitle class="text-left" v-text="'Price: ' + decimal(item.price)"></v-card-subtitle>
                <v-card-text class="text-left" v-text = "item.listingDescription"></v-card-text>
                <v-card-actions>
                  <v-btn color="green accent-4">View Listing</v-btn>
                  <!-- <v-btn color="green accent-4">Contact Host</v-btn> -->
                </v-card-actions>
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
</div>
</template>

<script>
export default {
  props: ['households'],
  computed: function () { return this.households },
  methods: {
    decimal: function (price) {
      return Number.parseFloat(price).toFixed(2)
    }
  }
}
</script>
