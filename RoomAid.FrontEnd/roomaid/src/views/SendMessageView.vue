<template>
<div id = "SendMessageView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavBar/>
  <div class="messages"> <!-- Start inbox container -->
    <!-- Start message items -->
  </div>
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavBar from '@/components/InboxNavBar.vue'

export default {
  name: 'ReadMessageView',
  components: {
    InboxNavBar
  },
  data: () => ({

  }),
  methods: {

  }
}
</script>

<style scoped>

</style>
