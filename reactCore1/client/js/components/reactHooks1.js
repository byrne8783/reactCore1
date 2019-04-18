import React, { useState, useEffect } from 'react';

export default function UserGreeting(props) {
    const [loading, setLoading] = useState(true);	        //(1) Using a state hook
    const [currentUser, setUser] = useState({"name":""});	//(2) Using a state hook
    const [error, setError] = useState(false);		        //(3) Using a state hook
    const [errorDetails, setErrorDetails] = useState("");	//(3a) Using a state hook

    useEffect(() => {								        //(4) Using an effect hook
        fetch('api/User?userId=' + props.UserId, {accept:'application/json'})
            .then(response => {
                if (response.status >= 200 && response.status < 300) {
                    return response;
                }
                const error = new Error(`HTTP Error ${response.statusText}`);
                error.status = response.statusText;
                error.response = response;
                console.log(error); // eslint-disable-line no-console
                throw error;
            })
            .then(response => response.json())
            .then(data => {
                setLoading(false);					//(later) update an existing state (1)
                setUser(data.currentUser);			//(later) update an existing state (2)
            })
            .catch(error => {
                setLoading(false);					//(later) maybe update an existing state (1)
                setError(true);					    //(later) maybe update an existing state (3)
                setErrorDetails(error.toString());	//(later) maybe update an existing state (3a)
            });
    }, [props.userId]);
    if (loading) {
        return "<p>Loading...<p>"
    }
    if (error) {
        return '<p>Error :' + errorDetails + '<p>'
    }
    return <p>Hello {currentUser.name}! I know its you because I stored you in local state using useState and got your name from a fetch in a useEffect hook.  I'm glad to see you.</p>
}
UserGreeting.defaultProps = {
    UserId: "Fred"
};
export { UserGreetings };


