<template>
    <div id="inboxnavbar">
        <nav>
        <v-navigation-drawer v-model="drawer" dark app class="grey darken-3 py-0">
            <v-container>
                <v-row>
                    <v-col md="9">
                        <v-icon right class="options"></v-icon> <!-- Add 'New' button for future implementation of new message creation from within inbox-->
                    </v-col>
                </v-row>
            </v-container>
            <v-list nav>
                <v-list-item router to="/inbox/messages">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Messages</v-list-item-title>
                    </v-list-item-content>
                    <v-chip
                        v-if="numUnread > 0"
                        color="green accent-3"
                        small>{{numUnread}}
                    </v-chip>
                </v-list-item>
                <v-list-item router to="/inbox/invitations">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Invitations</v-list-item-title>
                    </v-list-item-content>
                    <v-chip
                        v-if="numUnread > 0"
                        color="green accent-3"
                        small>{{numUnread}}
                    </v-chip>
                </v-list-item>
                <v-list-item router to="/inbox/sent">
                    <v-list-item-action>
                        <v-icon small></v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title class="text-left">Sent</v-list-item-title>
                    </v-list-item-content>
                </v-list-item>
                <!-- <v-list-item router to="/inbox/trash">
                    <v-list-item-action>
                        <v-icon small>fas fa-tachometer-alt</v-icon>
                    </v-list-item-action>
                    <v-list-item-content>
                        <v-list-item-title>Trash</v-list-item-title>
                    </v-list-item-content>
                </v-list-item> -->
            </v-list>
        </v-navigation-drawer>
    </nav>
    </div>
</template>

<script>
export default {
  name: 'InboxNavBar',
  data: () => ({
    drawer: true,
    numUnread: 1
  }),
  beforeDestroy () {
  },
  mounted () {
  },
  methods: {
  }
}
</script>

<style scoped>
.v-progress-circular {
    margin: 1rem;
}
</style>
