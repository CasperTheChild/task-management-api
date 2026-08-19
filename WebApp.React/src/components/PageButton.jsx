export default function PageButton({ setValue, val }) {
    return (
        <button onClick={() => setValue(val)} >
            Set to {val}
        </button>
    )
}