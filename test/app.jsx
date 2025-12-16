import { useState, useRef } from "react";
import { Stage, Layer, Rect, Text } from "react-konva";

export default function App() {
  const stageRef = useRef(null);
  const [vars, setVars] = useState({
    callsign: "KJ5LVN",
    operatorName: "David",
    qth: "Sapulpa, Oklahoma",
    grid: "EM25",
    ituZone: "7",
    cqZone: "4",
    dateUTC: "2025-12-16",
    timeUTC: "02:15",
    band: "20m",
    mode: "SSB",
    rst: "59",
  });

  const exportPNG = () => {
    const uri = stageRef.current.toDataURL({ pixelRatio: 2 });
    const a = document.createElement("a");
    a.href = uri;
    a.download = `eQSL_${vars.callsign}.png`;
    a.click();
  };

  const width = 1200,
    height = 800;

  return (
    <div style={{ display: "grid", gridTemplateColumns: "1fr 320px", gap: 16 }}>
      <div>
        <Stage width={width} height={height} ref={stageRef}>
          <Layer>
            <Rect x={0} y={0} width={width} height={height} fill="#0a0a0f" />
            <Text
              text={vars.callsign}
              x={40}
              y={40}
              fontSize={96}
              fontFamily="Orbitron"
              fill="#19f5ff"
              shadowColor="#19f5ff"
              shadowBlur={20}
              draggable
            />
            <Text
              text={`${vars.operatorName} • ${vars.qth} • Grid ${vars.grid}`}
              x={40}
              y={160}
              fontSize={28}
              fontFamily="Inter"
              fill="#d7eaff"
              draggable
            />
            <Text
              text={`QSO: ${vars.dateUTC} ${vars.timeUTC} UTC • ${vars.band} • ${vars.mode} • RST ${vars.rst}`}
              x={40}
              y={210}
              fontSize={24}
              fontFamily="Inter"
              fill="#9fb6ff"
              draggable
            />
            <Text
              text={`ITU ${vars.ituZone} • CQ ${vars.cqZone}`}
              x={40}
              y={250}
              fontSize={22}
              fontFamily="Inter"
              fill="#7aa0ff"
              draggable
            />
          </Layer>
        </Stage>
        <div style={{ marginTop: 12 }}>
          <button onClick={exportPNG}>Export PNG</button>
        </div>
      </div>

      <div>
        <h3>Variables</h3>
        {Object.entries(vars).map(([k, v]) => (
          <div key={k} style={{ marginBottom: 8 }}>
            <label style={{ display: "block", fontSize: 12 }}>{k}</label>
            <input
              value={v}
              onChange={(e) => setVars({ ...vars, [k]: e.target.value })}
              style={{ width: "100%" }}
            />
          </div>
        ))}
      </div>
    </div>
  );
}
