<template>
  <div id="inboxcontainer">
      <v-layout>
        <v-flex>
          <v-app style="background-color: white;" class="ma-n4">
            <v-container>
              <template>
                <v-data-table v-model="selected"
                :headers="headers"
                :items="messages"
                item-key="id"
                show-select
                sort-by="sentdate"
                @click:row="goToMessage"
                class="messagelist">
                </v-data-table>
              </template>
            </v-container>
          </v-app>
        </v-flex>
      </v-layout>
    </div>
</template>

<script>
export default {
  name: 'InboxContainer',
  data: () => ({
    messages: [],
    selected: [],
    headers: [
      { text: 'Name', sortable: true, value: 'name' },
      { text: 'Date', sortable: true, defaultSortBy: true, value: 'sentdate' }
    ]
  }),
  created () {
    this.initialize()
  },
  computed: {
  },
  methods: {
    initialize () {
      this.messages = [
        { id: 1, name: 'Bojack Horseman', sentdate: '04/04/2019' },
        { id: 2, name: 'Diane Nguyen', sentdate: '03/05/2018' }
      ]
    },
    getMessages () {
      const req = new Request(this.$hostname, {
        method: 'GET',
        headers: {
          Accept: 'application/json'
        },
        mode: 'cors'
      })
      fetch(req)
    },
    goToMessage (item) {
      this.$router.push(`/inbox/message/${item.id}`)
    }
  }
}
</script>

<!-- # for ids, . for classes; <span> for inline <div> to style individually-->
<style scoped>
#inboxcontainer {
    display:flex;
    position: relative;
    left: 128px;
}

::v-deep tbody tr {
  cursor: pointer;
}
</style>
