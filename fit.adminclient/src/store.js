import { createStore } from 'vuex'   // npm install vuex

export default createStore({
    state() {
        return {
            user: null
        }
    },
    mutations: {
        authenticate(state, userdata) {
            if (!userdata) {
                state.user = null;
                return;
            }
            state.user = userdata;
        }
    }
});
