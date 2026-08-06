import { useState } from "react";

export default function CreateTodoListForm({ onCreate }) {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [startDate, setStartDate] = useState("");

    function handleSubmit(event) {
        event.preventDefault();
        console.log("Creating item:", title, description, startDate);

        const newTodo = { title, description, startDate };

        onCreate(newTodo);

        setTitle("");
        setDescription("");
        setStartDate("");
    }

    return (<div>
        <h1>Create a todo list</h1>
        <form onSubmit={handleSubmit}>
            <label htmlFor="title">Title</label>
            <input
                id="title"
                type="text"
                name="title"
                value={title}
                onChange={e => setTitle(e.target.value)}></input>

            <label htmlFor="description">Description</label>
            <input
                id="description"
                type="text"
                name="description"
                value={description}
                onChange={e => setDescription(e.target.value)}></input>

            <label htmlFor="startDate">StartDate</label>
            <input
                id="startDate"
                type="date"
                name="startDate"
                value={startDate}
                onChange={e => setStartDate(e.target.value)}></input>

            <button type="submit">Create</button>
        </form>
    </div>
    )
}