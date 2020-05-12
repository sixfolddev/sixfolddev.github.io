<template>
<div id = "ReplyMessageView"> <!-- Start inbox container -->
  <v-content>
    <v-container fluid>
      <v-layout row>
        <v-flex md0><InboxNavPane/></v-flex>
        <v-flex md12><NewMessageBox/></v-flex>>
      </v-layout>
    </v-container>
  </v-content>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavPane from '@/components/InboxNavPane.vue'
import NewMessageBox from '@/components/NewMessageBox.vue'

export default {
  name: 'ReplyMessageView',
  components: {
    InboxNavPane,
    NewMessageBox
  },
  data: () => ({

  }),
  methods: {
  }
}
</script>

<style scoped>

</style>
